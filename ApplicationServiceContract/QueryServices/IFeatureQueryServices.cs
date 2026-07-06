using Application.Common.BaseModel;
using Application.DTOs.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.QueryServices
{
    public interface IFeatureQueryServices
    {
        Task<GenericComplexResult<FeatureSearchModel, FeatureListItem>> Search(FeatureSearchModel sm);
    }
}
