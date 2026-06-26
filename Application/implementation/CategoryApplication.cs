using Application.Common.BaseModel;
using Application.Contrast.Services;
using Application.DTOs.Category;
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
        public CategoryApplication(ICategoryRepository repo)
        {
            this.repo = repo;

        }
        private Category ToDBModel(CategoryAddModel model)
        {
            Category cat =
                 new Category ();

            cat.CategoryName = model.CategoryName;
            cat.CategoryDescription = model.CategoryDescription;
            cat.ParentId = model.ParentId;

            return cat;
             
        }





        public async Task<OperationResult> AddNew(CategoryAddModel model)//Check name
        {



            if (await repo.ExistSlugName(model.CategoryName))
            {
                return OperationResult.ToFail("add failed", new List<string>() { "name Already Exist" });
            }

            int depth = 0;
            if (model.ParentId == null) // setting max 3 depth
            {
                depth = 1;
            }
            if (model.ParentId != null)
            {
                Category parent = await repo.Get(model.ParentId.Value);
                if (parent == null)
                {
                    return OperationResult.ToFail("Add Failed", new List<string> { "this parent dose not exist" });
                    
                }
                if (parent.Depth == 3)
                {
                    return OperationResult.ToFail("add failed", new List<string>() { "Depth can't be more than 3 " });

                }
                depth = parent.Depth.Value + 1;

            }

            int recordId;
            try
            {
                var cat = ToDBModel(model);

                recordId = await repo.AddNew(cat);



            }
            catch
            {

                return OperationResult.ToFail("Add Failed", new List<string>());
            }
            string Lineage="";
            if (model.ParentId == null)
            {
                Lineage = $"{recordId},";

            }
            else
            {

                var parent = await repo.Get(model.ParentId.Value);
                Lineage = $"{parent.LineAge}{recordId},";
            }


            var l = repo.SetDepthAndLineAge(recordId, depth, Lineage);

            return OperationResult.ToSuccess(recordId, "Category Added");

        }

        public async Task<OperationResult> AssignFeatureToCategory(int categoryId, int FeaturId)
        {
            try
            {
                if (await repo.ExistCategoryFeature(categoryId, FeaturId))
                {
                    return OperationResult.ToFail("assign failed", new List<string> { "This Feature Already assigned" });
                }
                var cat = await repo.Get(categoryId);
                if (cat.Depth != 3)
                {
                    return OperationResult.ToFail("assign failed", new List<string> { " feature can only assign to level 3 category (depth must be three)" });

                }


                var catFeatureID = await repo.AssignFeatureToCategory(categoryId, FeaturId);
                return OperationResult.ToSuccess(catFeatureID, "Feature successfuly assigned");


            }
            catch
            {
                return OperationResult.ToFail("Feature successfuly assigned", new List<string>());

            }
        }

        public async Task<GenericOperationResult<List<CategoryUpdateModel>>> Get(int Categoryid)
        {
            try
            {
                var cat = await repo.Get(Categoryid);
                if (cat == null)
                {
                    return GenericOperationResult<List<CategoryUpdateModel>>.ToFail("Get Failed", new List<string> { "this category dose not exist" });
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
              
                return GenericOperationResult<List<CategoryUpdateModel>>.ToSuccess(Categoryid , "" ,new List<CategoryUpdateModel> { ViewCat });


            }
            catch
            {

                return GenericOperationResult<List<CategoryUpdateModel>>.ToFail("Get Failed", new List<string> { "can not find this category" });

            }
        }

        public async Task<OperationResult> Remove(int id)
        {
            //sub cat
            if (await repo.HasChildCategory(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category Has Child" });

            }
            //sub product
            if (await repo.HasProductCategory(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category Has Product" });

            }
            //if this cat exist
            if (!await repo.IsCategoryExist(id))
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "Category dose not exist" });

            }

            try
            {

                await repo.Remove(id);
                return OperationResult.ToSuccess(id, "Category Removed successfuly ");
            }
            catch
            {
                return OperationResult.ToFail(id, "remove failed", new List<string> { "can not remove this category" });


            }

        }

        public async Task<OperationResult> RemoveFeatureToCategory(int categoryId, int FeaturId)
        {
            try
            {
                var id = await repo.RemoveFeatureToCategory(categoryId,FeaturId);
                return OperationResult.ToSuccess(id, "Feature removed from Category");
            }
            catch
            {
                return OperationResult.ToFail("remove failed", new List<string> { "there is a problem to remove this feature " });

            }
        }

        public async Task<GenericComplexResult<CategorySearchModel, CategoryListItem>> Search(CategorySearchModel sm)
        {

           
            var cat = await repo.Search(sm);
            
            return cat;
            
          

        }

        public async Task<OperationResult> Update(CategoryUpdateModel model)
        {
            string OldParentLineAge = "";
            int NewParentDepth;
            string NewParentLineAge = "";

                var cat = new Category
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                CategoryDescription = model.CategoryDescription,
                ParentId = model.ParentId

            };
            // there is this model in db
            var oldnode = await repo.Get(model.CategoryId);

            //check if name is unique

            if (await repo.ExistCategoryName(cat.CategoryName))
            {
                return OperationResult.ToFail(cat.CategoryId, "Update Fialed", new List<string> { " this name already ssigned to another category" });
            }



            // avoid circular relations 

            //checked if the new parent id is exist

            if (oldnode.ParentId != cat.ParentId)
            {
                
                var oldparent = await repo.Get(oldnode.ParentId ?? 0 );
                if (oldparent != null)
                {
                    OldParentLineAge = oldparent.LineAge;

                }
                else { OldParentLineAge = ""; }
                var NewparentNode = await repo.Get(cat.ParentId.Value);
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
                    return OperationResult.ToFail(cat.CategoryId, "Update Failed", new List<string> { " can not update becuase with this new parnt depth will exceed 3" });
                }

                var nodeChildes = await repo.GetChildren(cat.CategoryId);
                

                foreach (var child in nodeChildes)
                {
                    string NewLineAge = string.Empty;
                    if (string.IsNullOrEmpty(OldParentLineAge))
                    {
                        NewLineAge = NewParentLineAge + child.CategoryId + ",";

                    }
                    else
                    {

                        NewLineAge = child.LineAge.Replace(OldParentLineAge, NewParentLineAge);
                    }

                    int NewDepth = NewLineAge.Count(C => C == ',');
                    await repo.SetDepthAndLineAge(child.CategoryId, NewDepth, NewLineAge);

                }

                if (NewparentNode.LineAge.Contains($"{model.CategoryId}"))
                {
                    return OperationResult.ToFail(model.CategoryId, "Update Failed", new List<string> { " a node can not be it's own child " });
                }



            }





            if (await repo.Update(cat))
            {
                return OperationResult.ToSuccess(cat.CategoryId, "Category Updated");
            }
            return OperationResult.ToFail(cat.CategoryId, "Update Fialed", new List<string>());


        }
    }
}
