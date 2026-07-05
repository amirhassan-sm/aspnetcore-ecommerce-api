using Application.Common.BaseModel;
using Application.DTOs.Product;
using Application.DTOs.ProductFeatures;
using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IProductApplicationServiceContract
    {
        
        Task<GenericComplexResult<ProductSearchModel, ProductListIteam>> SearchProductAsync(ProductSearchModel model);
        Task<OperationResult> setSeoAsync(ProductSeoModel Product);
        Task<GenericOperationResult<ProductModel>> Get(int ProductId);
        Task<OperationResult> AddNew(ProductAddModel product);
        Task<OperationResult> Remove(int ProductId);
        Task<OperationResult> Update(ProductUpdateModel product);
        Task<GenericOperationResult<List<ProductFeaturesModel>>> GetAllProductFeatures(int ProductId);
        Task<OperationResult> AssignFeatureToProductAsync(int FeatureId, int ProductId);
        Task<OperationResult> RemoveFeaturesFromProductAsync(int FeatureId, int ProductId);

    }
}
