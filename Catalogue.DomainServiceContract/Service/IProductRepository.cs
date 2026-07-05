using Catalogue.Domain.Models;
using Framwork.Domain.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Service
{
    public interface IProductRepository : IBaseRepoModel<Product, int>
    {
        Task<bool> ExistProductNameAsync(string productName);
        Task<bool> ExistProductSlugAsync(string slug);
        Task<bool> ProductExistExceptCurrentNameAsync(int id ,string productName);
        Task<bool> ProductExistExceptCurrentSlugAsync( int id ,string slugName);
       
        Task<List<int>>GetProductCategoryFeatures(int ProductId); // assign while add

        Task AsignAllCategoryFeaturesToProduct(int ProductId,List<int> CategoryFeatureId);

        Task setSeoAsync(Product Product);

        Task<List<ProductFeature>> GetAllProductFeatures(int ProductId);

        Task<bool> IsCategoryLevel3(int CategoryId);
        Task<bool> IsCategoryExist(int CategoryId);
        Task<bool> IsPrductExist(int productId);
        Task AssignFeatureToProductAsync(int FeatureId,int ProductId);
        Task RemoveFeaturesFromProductAsync(int FeatureId , int ProductId );

        Task<bool> IsFeaureExistAsync(int FeatureId);
        Task<bool> IsProductFeatureExistAsync(int productid , int FeatureId);
       




    }
}
