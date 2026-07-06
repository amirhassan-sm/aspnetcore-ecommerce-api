using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.DTOs.Feature;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Catalogue.EF.Persistance.Querie
{
    public class FeatureQueries : IFeatureQueryServices
    {
        private readonly CatalogueContext db;
        public FeatureQueries(CatalogueContext db)
        {
            this.db = db;   
            
        }
        public async Task<GenericComplexResult<FeatureSearchModel, FeatureListItem>> Search(FeatureSearchModel sm)
        {
            GenericComplexResult<FeatureSearchModel, FeatureListItem> result = 
                new GenericComplexResult<FeatureSearchModel, FeatureListItem> ();

            var q = db.features.AsNoTracking().AsQueryable();
            if (sm.SearchPhrase!=null)
            {
                q = q.Where(x => x.FeatureName.Contains(sm.SearchPhrase) || (x.FeatureDescription!=null &&
                x.FeatureDescription.Contains(sm.SearchPhrase)));


            }
            

            var q2 = from items in q
                     select new FeatureListItem
                     {
                         Description=items.FeatureDescription,
                         FeatureId=items.FeatureId,
                         FeatureName=items.FeatureName,

                     };
            sm.RecordCount = await q2.CountAsync();
            q2 = q2.OrderByDescending(x => x.FeatureId).Skip(sm.pageSize * sm.PageIndex).Take(sm.pageSize);
            result.sm = sm;
            result.ListItems = await q2.ToListAsync();
            return result;






        }
    }
}
