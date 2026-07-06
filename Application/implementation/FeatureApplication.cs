using Application.Common.BaseModel;
using Application.Contrast.QueryServices;
using Application.Contrast.Services;
using Application.DTOs.Feature;
using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.implementation
{
    public class FeatureApplication : IFeatureApplicationServiceContract
    {
        private readonly IFeatureRepository repo;
        private readonly IFeatureQueryServices queryServices;
        private readonly ILogger<FeatureApplication> logger;
        public FeatureApplication(IFeatureRepository repo, IFeatureQueryServices queryServices, ILogger<FeatureApplication> logger)
        {
            this.repo = repo;
            this.queryServices = queryServices;
            this.logger = logger;
            
        }
        private Feature MapModelToFeature(FeatureModel model)
        {
            return new Feature
            {
                FeatureId= model.FeatureId,
                FeatureName= model.FeatureName,
                FeatureDescription= model.FeatureDescription,

            };


        }
        private Feature MapAddModelToFeature(FeatureAddModel model)
        {
            return new Feature
            {
               
                FeatureName = model.FeatureName,
                FeatureDescription = model.FeatureDescription,

            };


        }

        public async Task<OperationResult> AddNew(FeatureAddModel model)
        {
            try
            {
                if (await repo.IsFeatureNameExist(model.FeatureName))
                {
                    return OperationResult.ToFail("add failed", new List<string> { "this feature name alread exists" });

                }
                var id = await repo.AddNew(MapAddModelToFeature(model));
                return OperationResult.ToSuccess(id, "Feature Added");
            }
            catch (Exception ex) {
                logger.LogError(ex, "the Feature failed to add {FeatureName}", model.FeatureName);
                 return OperationResult.ToFail("add failed", new List<string> { "an unexpected  error occurred" });


            }


        }

        public async Task<GenericOperationResult<FeatureModel>> Get(int FeatureId)
        {
            try
            {
                if (!await repo.IsFeatureExist(FeatureId))
                {
                    return  GenericOperationResult<FeatureModel>.ToFail();


                }
            }
        }

        public Task<OperationResult> Remove(int FeatureId)
        {
            throw new NotImplementedException();
        }

        public Task<GenericComplexResult<FeatureSearchModel, FeatureListItem>> Search(FeatureSearchModel sm)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Update(FeatureModel model)
        {
            throw new NotImplementedException();
        }
    }
}
