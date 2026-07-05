using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.DTOs.Product;
using Application.DTOs.ProductFeatures;
using Catalogue.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Catalogue.EF.Persistance.Querie
{
    public class ProductQueries : IProductQueryServices
        {
        private readonly CatalogueContext db;
        public ProductQueries(CatalogueContext db)
        {
            this.db = db;
            
        }

        public List<ProductFeaturesModel> GetProductFeatureModels(List<ProductFeature> productFeatures)
        {
            var result = from items in productFeatures
                         join fetures in db.features
                         on items.FeatureId equals fetures.FeatureId
                         select new ProductFeaturesModel
                         {
                             ProductId=items.ProductId,
                             FeatureId = items.FeatureId,
                             FeatureName = fetures.FeatureName,
                             ProductFeatureId = items.ProductFeatureId,
                             Value = items.Value

                         };
            return  result.ToList();
        }

        public async Task<GenericComplexResult<ProductSearchModel, ProductListIteam>> SearchProductAsync(ProductSearchModel model)
        {
            GenericComplexResult<ProductSearchModel, ProductListIteam> result =
                new GenericComplexResult<ProductSearchModel, ProductListIteam>();

            var q = from items in db.products select items;
            if (model.ParentId!= null)
            {
                q = q.Where(x => x.CategoryId == model.ParentId);

            }
            if (!string.IsNullOrEmpty(model.Phrase))
            {
                q = q.Where(x => x.ProductName.Contains(model.Phrase) || x.ProductDescription.Contains(model.Phrase));

            }
            if (model.FromUnitePrice != null)
            {
                q = q.Where(x => x.BasePrice >= model.FromUnitePrice);


            }
            if (model.ToUnitePrice != null)
            {
                q = q.Where(x => x.BasePrice <= model.ToUnitePrice);
            }
            bool CategorySearched = false;
            if (!CategorySearched && model.Level3CategoryId != null)
            {
                q = q.Where(x => x.CategoryId == model.Level3CategoryId);
                CategorySearched = true;

            }
            if (!CategorySearched && model.Level2CategoryId != null)
            {
                var level2Cat = db.categories.FirstOrDefault(x => x.CategoryId == model.Level2CategoryId);
                if (level2Cat == null)
                {
                    q = q.Where(x => false);


                }
                else
                {


                    var nextLevelIds = db.categories.Where(x => x.LineAge.StartsWith(level2Cat.LineAge)).Select(x => x.CategoryId).ToList();
                    q = q.Where(prod => nextLevelIds.Contains(prod.CategoryId));
                    CategorySearched = true;
                }






            }

            if (!CategorySearched)
            {
                var level1Cat = db.categories.FirstOrDefault(x => x.CategoryId == model.Level1CategoryId);
                if (level1Cat == null)
                {
                    q = q.Where(x => false);

                }
                else
                {
                    var nextLevelIds = db.categories.Where(x => x.LineAge.StartsWith(level1Cat.LineAge)).Select(x => x.CategoryId).ToList();
                    q = q.Where(prod => nextLevelIds.Contains(prod.CategoryId));
                }

            }
            q=q.AsNoTracking();
          
            var q2 = from p in q
                     join cat in db.categories
                     on p.CategoryId equals cat.CategoryId
                     select new ProductListIteam
                     {
                         CategoryName = cat.CategoryName,
                         CategoryId = p.CategoryId,
                         availbleCount = p.AvailableProductCount,
                         ProducId = p.ProductId,
                         ProductDescription = p.ProductDescription,
                         ProductName = p.ProductName,
                         SuplierName = string.Empty,
                         UnitePrice = p.BasePrice
                     };


            model.RecordCount = await q.CountAsync();

            q2 = q2.OrderByDescending(x => x.ProducId)
                .Skip(model.PageIndex * model.pageSize)
                .Take(model.pageSize);

            result.sm = model;
            result.ListItems = await q2.ToListAsync();

            return result;

            
            


        }
    }
}
