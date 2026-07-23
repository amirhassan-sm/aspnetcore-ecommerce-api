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
                        return OperationResult.ToFail("add failed", new List<string> { "this feature name alread exists" },"FeatureName_Already_exist");

                    }
                    var id = await repo.AddNew(MapAddModelToFeature(model));
                    return OperationResult.ToSuccess(id, "Feature Added");
                }
                catch (Exception ex) {
                    logger.LogError(ex, "the Feature failed to add {FeatureName}", model.FeatureName);
                     return OperationResult.ToFail("add failed", new List<string> { "an unexpected  error occurred" }, "Exception_Occured");


                }


            }

        public async Task<GenericOperationResult<FeatureModel>> Get(int FeatureId)
        {
            try
            {
                
                if (!await repo.IsFeatureExist(FeatureId))
                {
                    return GenericOperationResult<FeatureModel>
                            .ToFail("faied to get", new List<string> { "this feature does not exist" }, "Featuer_Not_Exist");


                }
                var feature = await repo.Get(FeatureId);
                var result = new FeatureModel
                {
                    FeatureId = feature.FeatureId,
                    FeatureDescription = feature.FeatureDescription,
                    FeatureName = feature.FeatureName,

                };
                return GenericOperationResult<FeatureModel>.ToSuccess(FeatureId, "get succees", result);
            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to get {FeatureId}", FeatureId);
                return GenericOperationResult<FeatureModel>
                    .ToFail("faied to get", new List<string> { "an unexpected error occured" }, "Exception_Occured");


            }
        }

        public async Task<OperationResult> Remove(int FeatureId)
        {
            try
            {
                if (!await repo.IsFeatureExist(FeatureId))
                {
                    return OperationResult.ToFail("failed to remove", new List<string> {"this feature does not exist"},"Feature_Not_Exists");
                    
                }
                if (await repo.HasFeatureAssigned(FeatureId))
                {
                    return OperationResult.ToFail("failed to remove", new List<string> {"this feature assigned to a product or catergory"},"Feature_Assigned");
                    
                }
                await repo.Remove(FeatureId);

                return OperationResult.ToSuccess(FeatureId,"removed successfully");
               

            }
            catch (Exception ex) {
                logger.LogError(ex, "failed to remove feature {FeatureId}", FeatureId);

                return OperationResult.ToFail("failed to remove", new List<string> { "an unexpected error occured" },"Exception_Occured");



            }
        }

        public async Task<GenericComplexResult<FeatureSearchModel, FeatureListItem>> Search(FeatureSearchModel sm)
        {
            return await queryServices.Search(sm);
        }

        public async Task<OperationResult> Update(FeatureModel model)   
        {
            try {
                if (!await repo.IsFeatureExist(model.FeatureId))
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this Feature does not exists" }, "Featuer_Not_Exist");
                    
                }
                if (await repo.IsFeatureNameExistExceptCurrent(model.FeatureId,model.FeatureName))
                {
                    return OperationResult.ToFail("failed to update", new List<string> { "this name already exist" }, "Featuer_Already_Exist");


                }
               await repo.Update(MapModelToFeature(model));
                return OperationResult.ToSuccess("update succeed");



            }
            catch(Exception ex)
            {
                logger.LogError(ex, "failed to update feature {FeatureId} ", model.FeatureId);
                return OperationResult.ToFail("failed to update", new List<string> { "an unexpected error occured" }, "Excpetion_Occured");

            }
        }

     
    }
}
