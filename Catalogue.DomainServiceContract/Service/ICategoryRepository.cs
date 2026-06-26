
using Application.Common.BaseModel;
using Application.DTOs.Category;
using Catalogue.Domain.Models;
using Framwork.Domain.BaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Service
{
    public interface ICategoryRepository : IBaseRepoModel<Category,int>
    {
        Task<int> AssignFeatureToCategory(int categoryId,int FeaturId);
        Task<int> RemoveFeatureToCategory(int categoryId, int FeaturId);
        Task<GenericComplexResult<CategorySearchModel,CategoryListItem >> Search(CategorySearchModel sm);

        Task<bool> ExistCategoryName(string categoryName);
        Task<bool> ExistSlugName(string categorySlug);

        Task<bool> SetDepthAndLineAge(int CategoryId , int depth , string LineAge);
        Task<bool> ExistCategoryFeature(int Categoryid , int FeatureId );

        Task<bool> HasChildCategory(int CategoryId );

        Task<bool> HasProductCategory(int CategoryId);
        Task<bool> IsCategoryExist(int CategoryId);
        Task<int> GetChildNodeLevels(int CategoryID);

        Task<List<Category>> GetChildren(int CategoryID);

        //Task<List<CategoryFeature>> GetCategoryFeatures(int CategoryId);




    }
}
