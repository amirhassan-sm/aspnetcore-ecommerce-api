using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Service
{
    public interface IFeatureRepository
    {
        Task<Feature> Get(int id);
        Task<int> AddNew(Feature model);
        Task Remove(int id);
        Task Update(Feature model);

        Task<bool> IsFeatureExist(int FeatureId);
        Task<bool> IsFeatureNameExist(string FeatureName);
        Task<bool> IsFeatureNameExistExceptCurrent(int FeatureId,string FeatureName);
        Task<bool> HasFeatureAssigned(int FeatureId);

       



    }
}
