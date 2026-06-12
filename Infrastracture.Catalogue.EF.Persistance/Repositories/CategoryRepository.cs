using Application.Common.BaseModel;
using Application.DTOs.Category;
using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace Infrastracture.Catalogue.EF.Persistance.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogueContext db;
        public CategoryRepository(CatalogueContext db)
        {
            this.db = db;
        }

        public async Task<int> AddNew(Category model)
        {
            try
            {

                db.categories.Add(model);
                await db.SaveChangesAsync();
                return model.CategoryId;
            }

            catch (Exception ex) { 
            
            return -1;
            }



        }

        public async Task<int> AssignFeatureToCategory(int categoryId, int FeaturId)
        {
            CategoryFeature categoryFeature = new CategoryFeature { CategoryId = categoryId,FeatureId=FeaturId};
           
                db.CategoryFeatures.Add(categoryFeature);
               await db.SaveChangesAsync();
                return categoryFeature.CategoryFeatureId ;
            
            
        }

        public async Task<bool> ExistCategoryFeature(int Categoryid, int FeatureId)
        {
            return await db.CategoryFeatures.AnyAsync(x=> x.CategoryId == Categoryid && x.FeatureId == FeatureId);
        }

        public async Task<bool> ExistCategoryName(string categoryName)
        {
            return await db.categories.AnyAsync(x=>x.CategoryName == categoryName);
                
        }

        public async Task<bool> ExistSlugName(string categorySlug)
        {
            return await db.categories.AnyAsync(x => x.Slug == categorySlug);
        }

        public async Task<Category> Get(int id)
        {
            var cat = await db.categories.FirstOrDefaultAsync(x=>x.CategoryId==id);

            
            return cat;
        }

        public async Task<bool> HasChildCategory(int CategoryId)
        {
            return await db.categories.AnyAsync(x=>x.ParentId == CategoryId);
        }

        public async Task<bool> HasProductCategory(int CategoryId)
        {
            return await db.products.AnyAsync(x=>x.CategoryId == CategoryId);
        }

        public async Task<bool> IsCategoryExist(int CategoryId)
        {
            return await db.categories.AnyAsync(x=>x.CategoryId==CategoryId);
        }

        public async Task<bool> Remove(int id)
        {
            try {
                var cat = await db.categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                db.categories.Remove(cat);
                await db.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<int> RemoveFeatureToCategory(int categoryId, int FeaturId)
        {

            //var cf = await db.CategoryFeatures.FirstOrDefaultAsync(x=>x.FeatureId==FeaturId && x.CategoryId==categoryId);
            var cf = await db.CategoryFeatures.FirstOrDefaultAsync(x => x.FeatureId == FeaturId && x.CategoryId == categoryId);

            db.CategoryFeatures.Remove(cf);
                return cf.CategoryFeatureId;
            
           
        }

        public async Task<bool> SetDepthAndLineAge(int CategoryId, int depth, string LineAge)
        {
            try {
                var cat = db.categories.FirstOrDefault(x=>x.CategoryId == CategoryId);
                cat.Depth = depth;
                cat.LineAge = LineAge;
                await db.SaveChangesAsync();
                return true;
            
            }
            catch { 
            return false;
            }
        }

        public async Task<bool> Update(Category model)
        {
            try {
                var cat = await db.categories.FirstOrDefaultAsync(x=>x.CategoryId==model.CategoryId);
              
                    
                    cat.CategoryName = model.CategoryName;
                    cat.CategoryDescription = model.CategoryDescription;
                    cat.MetaData = model.MetaData;
                    cat.MetaTag=model.MetaData;
                    cat.Slug = model.Slug;
                    cat.ParentId = model.ParentId; 
                     
                    await db.SaveChangesAsync();

                    return true;

                


            }
            catch { 
                return false;
            
            }
        }

          async  Task<GenericComplexResult<CategorySearchModel, CategoryListItem>> ICategoryRepository.Search(CategorySearchModel sm)
        {

            GenericComplexResult<CategorySearchModel, CategoryListItem> result = new GenericComplexResult<CategorySearchModel, CategoryListItem>();
           var q = from c in db.categories select c ;

            
            if (!string.IsNullOrEmpty(sm.SeachPhrase))
            {
                q = q.Where(x => x.CategoryName.Contains(sm.SeachPhrase) || x.CategoryDescription.Contains(sm.SeachPhrase));
            }

            
            if (sm.ParentID > 0) 
            {
                q = q.Where(x => x.ParentId == sm.ParentID);
            }

   
            var q2 = from items in q
                     select new CategoryListItem
                     {
                         CategoryID = items.CategoryId,
                         CategoryName = items.CategoryName,
                         ProductCount = 1

                     } ;
            sm.RecordCount = await q.CountAsync();
            q2 =  q2.OrderByDescending(x => x.CategoryID).Skip(sm.PageIndex * sm.pageSize).Take(sm.pageSize);

            
            result.ListItems =  await q2.ToListAsync();
            result.sm = sm;
            

            return result;


        }

        public async Task<int> GetChildNodeLevels(int CategoryID)
        {
            var node = await db.categories.FirstOrDefaultAsync(x => x.CategoryId == CategoryID);
            int MaxDepth = await db.categories.Where(x => x.LineAge.StartsWith(node.LineAge)).MaxAsync(x => x.Depth) ?? 0;
            //we need to undrestand that the node we selected has how many child node
            //so max depth - depth of cruurent == depth of current node (the childs that current node has)

            var MaxChildDepth = MaxDepth - node.Depth.Value;
            return MaxChildDepth ;// +1 for current node it self
        }

        public async Task<List<Category>> GetChildren(int CategoryID)
        {
           var node = db.categories.FirstOrDefault(x=>x.CategoryId == CategoryID)   ;
            var cats = await db.categories.Where(x => x.LineAge.StartsWith(node.LineAge)).ToListAsync();
            return cats;
        }
    }
}
