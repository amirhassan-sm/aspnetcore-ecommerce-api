using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Category;
using Application.DTOs.CategoryFeatureModel;
using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class CategoryApplication : ICategoryApplicationServiceContract
    {
        private readonly ICategoryRepository repo;
        //private readonly IProductApplicationServiceContract prodApp;
        public CategoryApplication(ICategoryRepository repo)
        {
            this.repo = repo;

        }
        private Category ToDBModel(CategoryAddModel model)
        {
            Category cat =
                 new Category();

            cat.CategoryName = model.CategoryName;
            cat.CategoryDescription = model.CategoryDescription;
            cat.ParentId = model.ParentId;
            cat.Slug = model.Slug;

            return cat;

        }

        /// <summary>
        /// Resolves category depth when Depth is null (legacy/broken rows).
        /// </summary>
        private async Task<int> ResolveCategoryDepthAsync(Category category)
        {
            if (category.Depth.HasValue && category.Depth.Value > 0)
                return category.Depth.Value;

            if (!string.IsNullOrWhiteSpace(category.LineAge))
            {
                var segments = category.LineAge
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (segments.Length > 0)
                    return segments.Length;
            }

            if (category.ParentId == null)
                return 1;

            var parent = await repo.Get(category.ParentId.Value);
            if (parent == null)
                return 0;

            var parentDepth = await ResolveCategoryDepthAsync(parent);
            return parentDepth <= 0 ? 0 : parentDepth + 1;
        }

        private static string EnsureLineAge(Category category)
        {
            if (!string.IsNullOrWhiteSpace(category.LineAge))
            {
                return category.LineAge.EndsWith(",")
                    ? category.LineAge
                    : $"{category.LineAge},";
            }

            return $"{category.CategoryId},";
        }

        private Category UpdateModelMapToCat(CategoryUpdateModel model)
        {
            return new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                ParentId = model.ParentId,
                LineAge = model.LineAge,
                Depth = model.Depth,
                Slug = model.Slug,
                

            };
          

        }





        public async Task<OperationResult> AddNew(CategoryAddModel model)
        {
           



            if (await repo.ExistCategoryName(model.CategoryName))
            {
                return OperationResult.ToFail("add failed", new List<string>() { "name Already Exist" },"CategoryName_Already_Exist");
            }
            if (await repo.ExistSlugName(model.Slug))
            {
                return OperationResult.ToFail("add failed", new List<string> { "slug already exist" },"Slug_Already_Exist");


            }

            int depth;
            Category? parentForLineage = null;

            if (model.ParentId == null)
            {
                depth = 1;
            }
            else
            {
                Category parent = await repo.Get(model.ParentId.Value);
                if (parent == null)
                {
                    return OperationResult.ToFail(
                        "Add Failed",
                        new List<string> { "this parent dose not exist" },
                        "Parent_Is_Null");
                }

                // Depth is nullable in DB; older/broken rows may have null and previously crashed on .Value
                int parentDepth = await ResolveCategoryDepthAsync(parent);
                if (parentDepth <= 0)
                {
                    return OperationResult.ToFail(
                        "Add Failed",
                        new List<string> { "parent category depth is invalid" },
                        "Parent_Depth_Invalid");
                }

                // Persist repaired depth so next child creates do not fail again
                if (!parent.Depth.HasValue || parent.Depth.Value != parentDepth)
                {
                    var repairedLineAge = EnsureLineAge(parent);
                    await repo.SetDepthAndLineAge(parent.CategoryId, parentDepth, repairedLineAge);
                    parent.Depth = parentDepth;
                    parent.LineAge = repairedLineAge;
                }

                if (parentDepth >= 3)
                {
                    return OperationResult.ToFail(
                        "add failed",
                        new List<string> { "Depth can't be more than 3 " },
                        "Depth_Is_BiggerThan3");
                }

                depth = parentDepth + 1;
                parentForLineage = parent;
            }

            int recordId;
            try
            {
                var cat = ToDBModel(model);
                recordId = await repo.AddNew(cat);
            }
            catch
            {
                return OperationResult.ToFail(
                    "Add Failed",
                    new List<string> { "an unexpected error occured" },
                    "ExcetionThrows");
            }

            string lineage;
            if (parentForLineage == null)
            {
                lineage = $"{recordId},";
            }
            else
            {
                var parentLineAge = EnsureLineAge(parentForLineage);
                lineage = $"{parentLineAge}{recordId},";
            }

            var depthSaved = await repo.SetDepthAndLineAge(recordId, depth, lineage);
            if (!depthSaved)
            {
                return OperationResult.ToFail(
                    recordId,
                    "Category created but depth/lineage was not saved",
                    new List<string> { "SetDepthAndLineAge failed" },
                    "Depth_Persist_Failed");
            }

            return OperationResult.ToSuccess(recordId, "Category Added");

        }

        public async Task<OperationResult> AssignFeatureToCategory(int categoryId, int FeaturId)
        {
            try
            {
                if (!await repo.IsCategoryExist(categoryId))
                {
                    return OperationResult.ToFail("Failed to assign", new List<string> {"this category does not exist" }, "Categor_Not_exist");
                    
                }
                if (!await repo.IsFeatureExist(FeaturId) )
                {
                    return OperationResult.ToFail("Failed to assign", new List<string> { "this feature does not exist" },"Feature_Not_Exist");

                }
                if (await repo.ExistCategoryFeature(categoryId, FeaturId))
                {
                    return OperationResult.ToFail("assign failed", new List<string> { "This Feature Already assigned" },"CategoryFeature_Already_Exist");
                }

                var cat = await repo.Get(categoryId);
                
                if (cat.Depth != 3)
                {
                    return OperationResult
                        .ToFail("assign failed", new List<string> { " feature can only assign to level 3 category (depth must be three)" }
                        ,"Category_IsNot_Level3");

                }


                var catFeatureID = await repo.AssignFeatureToCategory(categoryId, FeaturId);

                var prodIds= await repo.GetCategoryProductsId(categoryId);
                    
                await repo.AssignFeatureToProductsAsync(FeaturId, prodIds); 


                
                return OperationResult.ToSuccess(catFeatureID, "Feature successfuly assigned");


            }
            catch(Exception ex)
            {
                return OperationResult.ToFail("assign failed", new List<string> { "an unexpected error occured" }, "Exception_Occured");

            }
        }

        public async Task<GenericOperationResult<List<CategoryUpdateModel>>> Get(int Categoryid)
        {
            try
            {
                var cat = await repo.Get(Categoryid);
                if (cat == null)
                {
                    return GenericOperationResult<List<CategoryUpdateModel>>
                        .ToFail("Get Failed", new List<string> { "this category dose not exist" },"Category_Not_Exist");
                }
                CategoryUpdateModel ViewCat = new CategoryUpdateModel
                {
                    CategoryDescription = cat.CategoryDescription,
                    CategoryName = cat.CategoryName,
                    CategoryId = cat.CategoryId,
                    Depth = cat.Depth,
                    LineAge = cat.LineAge,
                    ParentId = cat.ParentId,
                };

                return GenericOperationResult<List<CategoryUpdateModel>>.ToSuccess(Categoryid, "", new List<CategoryUpdateModel> { ViewCat });


            }
            catch(Exception ex)
            {

                return GenericOperationResult<List<CategoryUpdateModel>>.ToFail("Get Failed", new List<string> { "can not find this category" }
                ,"Execption_Occured");

            }
        }

        public async Task<OperationResult> Remove(int id)
        {
            //sub cat
            if (await repo.HasChildCategory(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category Has Child" },"Category_Has_Child");

            }
            //sub product
            if (await repo.HasProductCategory(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category Has Product" }, "Category_Has_Product");

            }
            //if this cat exist
            if (!await repo.IsCategoryExist(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category dose not exist" },"Category_DoesNot_Exist");

            }

            try
            {
                await repo.RemoveAllFeaturesFromCategory(id);

                await repo.Remove(id);
                return OperationResult.ToSuccess(id, "Category Removed successfully ");
            }
            catch
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "can not remove this category" }, "Exception_Occured");


            }

        }
        

        public async Task<OperationResult> RemoveFeatureToCategory(int categoryId, int FeaturId)
        {
            try
            {
                if (!await repo.IsCategoryExist(categoryId))
                {
                    return OperationResult.ToFail("failed to remove Category Feature", new List<string> { "this Category does not exist" }, "Category_Not_Exist");
                    
                }
                if (!await repo.IsFeatureExist(FeaturId))
                {
                    return OperationResult.ToFail("failed to remove Category Feature", new List<string> { "this Feature does not exist" }
                    , "Feature_Not_Exist");

                }
                if (! await repo.ExistCategoryFeature(categoryId,FeaturId))
                {
                    return OperationResult.ToFail("failed to remove Category Feature"
                        ,new List<string> {"this category feature does not exist"}, "CategoryFeature_DoesNot_Exist");
                    
                }
                var id = await repo.RemoveFeatureToCategory(categoryId, FeaturId);

                var prodIds = await repo.GetCategoryProductsId(categoryId);

                await repo.RemoveFeatureFromProductsAsync(FeaturId, prodIds);

                
                return OperationResult.ToSuccess(id, "Feature removed from Category");
            }
            catch(Exception ex)
            {
                return OperationResult.ToFail("remove failed", new List<string> { "there is a problem to remove this feature " }, "Exception_Throws");

            }
        }

        public async Task<GenericComplexResult<CategorySearchModel, CategoryListItem>> Search(CategorySearchModel sm)
        {


            var cat = await repo.Search(sm);

            return cat;



        }

        public async Task<OperationResult> Update(CategoryUpdateModel model)
        {
            //check parent and cat id not same 
            string OldParentLineAge = "";
            int NewParentDepth;
            string NewParentLineAge = "";

            var cat = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                ParentId = model.ParentId,
                Slug = model.Slug,



            };
            
            var oldnode = await repo.Get(model.CategoryId);
            if (oldnode ==null) {

                return OperationResult.ToFail(cat.CategoryId , "Update Fialed", new List<string> {"this category does not exist"}, "Category_Not_Exist");
            
            }

            

            if (await repo.ExistCategoryNameExceptCurrentAsync(cat.CategoryId,cat.CategoryName))
            {
                return OperationResult.ToFail(cat.CategoryId, "Update Fialed", new List<string> { " this name already ssigned to another category" }, "CategoryName_Already_exist");
            }

            
            if (await repo.ExistCategorySlugExceptCurrentAsync(cat.CategoryId,cat.Slug))
            {
                return OperationResult.ToFail("update failed", new List<string> { " this slug already exist " },"Category_Slug_AlreadyExist");
            }

            if (model.ParentId == model.CategoryId)
            {
                return OperationResult.ToFail(
                    model.CategoryId,
                    "Update Failed",
                    new List<string>
                    {
            "A category cannot be its own parent."
                    },
                    "Category_CannotBe_OwnParent");
            }







            if (oldnode.ParentId != cat.ParentId) 
            {


                var oldparent = await repo.Get(oldnode.ParentId ?? 0);
                if (oldparent != null)
                {
                    OldParentLineAge = oldparent.LineAge;

                }
                else { OldParentLineAge = ""; }




                  
                var NewparentNode = await repo.Get(cat.ParentId ??0);
                if (NewparentNode != null)
                {
                    NewParentLineAge = NewparentNode.LineAge;
                    NewParentDepth = NewparentNode.Depth.Value;


                }
                else
                {
                    NewParentLineAge = "";
                    NewParentDepth = 1;
                }

                var ChildrenRealativeDepth = await repo.GetChildNodeLevels(cat.CategoryId);
                if (ChildrenRealativeDepth + NewParentDepth > 3)
                {
                    return OperationResult.ToFail(cat.CategoryId, "Update Failed",
                        new List<string> { " can not update becuase with this new parnt depth will exceed 3" },"Depth_Exceed_Limit");
                }

                
                if (NewparentNode?.LineAge?.Contains($"{model.CategoryId}," )==true)
                {
                    return OperationResult.ToFail(model.CategoryId, "Update Failed", new List<string> { " a node can not be it's own child " }
                    ,"loop_tree");
                }
                var nodeChildes = await repo.GetChildren(cat.CategoryId);


                foreach (var child in nodeChildes)
                {
                    string NewLineAge = string.Empty;
                    if (string.IsNullOrEmpty(OldParentLineAge))
                    {
                        NewLineAge = NewParentLineAge + child.LineAge ;

                    }
                    else
                    {

                        NewLineAge = child.LineAge.Replace(OldParentLineAge, NewParentLineAge);
                    }

                    int NewDepth = NewLineAge.Count(C => C == ',');
                    await repo.SetDepthAndLineAge(child.CategoryId, NewDepth, NewLineAge);

                }





            }





            if (await repo.Update(cat))
            {
                return OperationResult.ToSuccess(cat.CategoryId, "Category Updated");
            }
            return OperationResult.ToFail(cat.CategoryId, "an unexpected error occured", new List<string>(), "Exception_Occured");


        }

        public async Task<GenericOperationResult<List<CategoryFeatureModel>>> GetCategoryFeatures(int CategoryId)
        {
            if (!await repo.IsCategoryExist(CategoryId))
            {
                return GenericOperationResult<List<CategoryFeatureModel>>.ToFail("failed to get", new List<string>
            {"this category does not exist"},"Category_Not_Exist");


            }
            try
            {
                var categoryFeatures = await repo.GetCategoryFeatures(CategoryId);
                var result = categoryFeatures.Select(x => new CategoryFeatureModel
                {
                    CategoryFeatureId = x.CategoryFeatureId,
                    CategoryId = x.CategoryId,
                    FeatureId = x.FeatureId,

                }).ToList();

                return GenericOperationResult<List<CategoryFeatureModel>>.ToSuccess("get succeed", result);
            }
            catch (Exception ex) {
                return GenericOperationResult<List<CategoryFeatureModel>>.ToFail("failed to get", new List<string>
            {"an unexpected error occures"},"Exception_Occured");

            }






        }

        public async Task<GenericOperationResult<List<CategoryUpdateModel>>> GetLevel1CategoryAsync()
        {
            try
            {
                var rootCats = await repo.GetLevel1CategoryAsync();
                var result = rootCats.Select(x => new CategoryUpdateModel
                {
                    CategoryDescription = x.CategoryDescription,
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    Depth = x.Depth,
                    LineAge = x.LineAge,
                    ParentId = x.ParentId,
                    Slug = x.Slug,

                }).ToList();
                return GenericOperationResult<List<CategoryUpdateModel>>.ToSuccess("root categories",result);

            }
            catch (Exception ex) 
            {
               return GenericOperationResult<List<CategoryUpdateModel>>.ToFail("failed to get root categories", new List<string> {
                "an unexpected error occured"
                
                },"Exception_Occured");
            }
        }
    }
}
