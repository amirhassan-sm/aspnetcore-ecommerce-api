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
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogueContext db;
        public ProductRepository(CatalogueContext db)
        {
            this.db = db;
        }
        public async Task<int> AddNew(Product model)
        {

            db.products.Add(model);
            await db.SaveChangesAsync();

            return model.ProductId;





        }

        public async Task AsignAllCategoryFeaturesToProduct(int ProductId, List<int> CategoryFeatureId)
        {
            var features = CategoryFeatureId.Select(x => new ProductFeature
            {
                FeatureId = x,
                ProductId = ProductId,
                Value = string.Empty



            });
            db.ProductFeatures.AddRange(features);

            await db.SaveChangesAsync(); ;




        }

        public async Task AssignFeatureToProductAsync(int FeatureId, int ProductId)
        {
         

            var result = db.ProductFeatures.Add(new ProductFeature {
                FeatureId = FeatureId,
                ProductId= ProductId,
                Value= string.Empty
            
            
            });
            await db.SaveChangesAsync();
            
        }

        public async Task<bool> ExistProductNameAsync(string productName)
        {
            return await db.products.AnyAsync(x => x.ProductName == productName);
        }

        public async Task<bool> ExistProductSlugAsync(string slug)
        {
            return await db.products.AnyAsync(y => y.Slug == slug);
        }

        public Task<Product?> Get(int id)
        {
            return db.products.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<List<ProductFeature>> GetAllProductFeatures(int ProductId)
        {
            var fetures = db.ProductFeatures.Where(x=>x.ProductId == ProductId);
            return await fetures.ToListAsync();
        }

        public async Task<List<int>> GetProductCategoryFeatures(int ProductId)
        {
            var prod = await db.products.FirstOrDefaultAsync(x => x.ProductId == ProductId);
            if (prod == null)
            {
                return new List<int>();
            }

            var list = await db.CategoryFeatures.Where(x => x.CategoryId == prod.CategoryId)
                .Select(x => x.CategoryFeatureId).ToListAsync();

            return list;

        }

        public async Task<bool> IsCategoryExist(int CategoryId)
        {
            var cat = await db.categories.FirstOrDefaultAsync(x => x.CategoryId == CategoryId);
            if (cat == null)
            {
                return false;

            }
            else return true; 

        }

        public async Task<bool> IsCategoryLevel3(int CategoryId)
        {
            var cat = await db.categories.FirstOrDefaultAsync(x=>x.CategoryId == CategoryId);
            if (cat==null)
            {
                return false;
                
            }
            
            if (cat.Depth != 3 )
            {
                return false;
                
            }
            return true;

        }

        public Task<bool> IsFeaureExistAsync(int FeatureId)
        {
            return db.features.AnyAsync(x=>x.FeatureId==FeatureId);
        }

        public async Task<bool> IsPrductExist(int productId)
        {
            return await db.products.AnyAsync(x=>x.ProductId == productId);
        }

        public async Task<bool> IsProductFeatureExistAsync(int productid, int FeatureId)
        {
           return await db.ProductFeatures.AnyAsync(x=>x.ProductId == productid && x.FeatureId == FeatureId);
        }

        public async Task<bool> ProductExistExceptCurrentNameAsync(int id, string productName)
        {
            return await db.products.AnyAsync(x => x.ProductId != id && x.ProductName == productName);
        }

        public async Task<bool> ProductExistExceptCurrentSlugAsync(int id, string slugName)
        {
            return await db.products.AnyAsync(x => x.ProductId != id && x.Slug == slugName);
        }

        public async Task<bool> Remove(int id)
        {

            var prod = await db.products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (prod == null)
            {
                return false;

            }
            db.products.Remove(prod);
            await db.SaveChangesAsync();
            return true;



        }

        public async Task RemoveAllFeaturesFromProduct(int id)
        {
            var features = db.ProductFeatures.Where(x=>x.FeatureId==id);
            db.RemoveRange(features);
            await db.SaveChangesAsync();
        }

        public async Task RemoveFeaturesFromProductAsync(int FeatureId, int ProductId)
        {
            var ProdFeature = await db.ProductFeatures.FirstOrDefaultAsync(x => x.FeatureId==FeatureId && x.ProductId==ProductId);
            
            db.ProductFeatures.Remove(ProdFeature);
            await db.SaveChangesAsync();
           
        }

        public async Task SetFeatureValues(int productId, int fetureId, string value)
        {
            var fetuer = db.ProductFeatures.FirstOrDefault(x=>x.FeatureId == fetureId&& x.ProductId==productId);
            fetuer.Value = value;
            await db.SaveChangesAsync();
        }

        public async Task setSeoAsync(Product Product)
        {
           var prod = await db.products.FirstOrDefaultAsync(x=>x.ProductId == Product.ProductId);
      

            prod.ExtraData = Product.ExtraData;
            prod.BodyExtraData = Product.BodyExtraData;
            prod.Slug = Product.Slug;
            prod.PageTitle = Product.PageTitle;
            prod.MetaTag = Product.MetaTag;
            prod.MetaDescription = Product.MetaDescription;

            await db.SaveChangesAsync();
           
        }


        

        public async Task<bool> Update(Product model)
        {
            var prod = await db.products.FirstOrDefaultAsync(x=>x.ProductId == model.ProductId);
            if (prod == null)
            {
                return false;

                
            }
            prod.ProductName = model.ProductName;
            prod.ProductDescription = model.ProductDescription;
            prod.BasePrice = model.BasePrice;
            prod.Introduction = model.Introduction;
            prod.ExtraData = model.ExtraData;
            prod.ImageUrl = model.ImageUrl;
            prod.BodyExtraData  = model.BodyExtraData;
            prod.AvailableProductCount = model.AvailableProductCount;
            prod.Slug = model.Slug;
            prod.PageTitle  = model.PageTitle;
            prod.MetaTag = model.MetaTag;
            prod.MetaDescription = model.MetaDescription;
            await db.SaveChangesAsync();
            return true;
            

        }
    }
}
