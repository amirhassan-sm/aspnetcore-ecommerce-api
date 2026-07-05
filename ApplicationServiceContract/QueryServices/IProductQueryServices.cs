using Application.Common.BaseModel;
using Application.DTOs.Product;
using Application.DTOs.ProductFeatures;
using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.QueryServices
{
    public interface IProductQueryServices
    {
        Task<GenericComplexResult<ProductSearchModel,ProductListIteam>> SearchProductAsync(ProductSearchModel model);
        List<ProductFeaturesModel> GetProductFeatureModels(List<ProductFeature> productFeatures);
    }
}
