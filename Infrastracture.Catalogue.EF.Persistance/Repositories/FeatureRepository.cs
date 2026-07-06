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
    public class FeatureRepository : IFeatureRepository
    {
        private readonly CatalogueContext db;
        public FeatureRepository(CatalogueContext db)
        {
            this.db = db;
            
        }
        public async Task<int> AddNew(Feature model)
        {
            db.features.Add(model);
            await db.SaveChangesAsync();
            return model.FeatureId;
            
            
        }

        public async Task<Feature> Get(int id)
        {
            return await db.features.FindAsync(id);
        }

        public async Task<bool> HasFeatureAssigned(int FeatureId)
        {
            bool CheckCat = await db.CategoryFeatures.AnyAsync(x=>x.FeatureId == FeatureId);
            bool checkProd = await db.ProductFeatures.AnyAsync(x=>x.FeatureId == FeatureId);
            if (CheckCat || checkProd) {
                return true;
            
            
            }
            return false;
        }

        public async Task<bool> IsFeatureExist(int FeatureId)
        {
            return await db.features.AnyAsync(x=>x.FeatureId== FeatureId);
        }

        public async Task<bool> IsFeatureNameExist(string FeatureName)
        {
            return await db.features.AnyAsync(x=>x.FeatureName == FeatureName);
        }

        public async Task<bool> IsFeatureNameExistExceptCurrent(int FeatureId, string FeatureName)
        {
            return await db.features.AnyAsync(x=>x.FeatureId!=FeatureId && x.FeatureName==FeatureName);
        }

        public async Task Remove(int id)
        {
            var feature = await db.features.FirstOrDefaultAsync(x=>x.FeatureId==id);
            db.features.Remove(feature);

            await db.SaveChangesAsync();
        }

        public async Task Update(Feature model)
        {
            var feature = await db.features.FirstOrDefaultAsync(x => x.FeatureId == model.FeatureId);
            feature.FeatureDescription= model.FeatureDescription;
            feature.FeatureName= model.FeatureName;
            await db.SaveChangesAsync();
        }
    }
}
