using Application.Common.BaseModel;
using Application.DTOs.Feature;
using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IFeatureApplicationServiceContract
    {
        Task<GenericOperationResult<FeatureModel>> Get(int FeatureId);
        Task<OperationResult> AddNew(FeatureAddModel model);
        Task<OperationResult> Remove(int FeatureId);
        Task<OperationResult> Update(FeatureModel model);
        Task<GenericComplexResult<FeatureSearchModel, FeatureListItem>> Search(FeatureSearchModel sm);
        

    }
}
