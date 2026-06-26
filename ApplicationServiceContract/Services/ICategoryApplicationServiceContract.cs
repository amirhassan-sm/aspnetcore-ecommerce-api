using Application.Common.BaseModel;
using Application.DTOs.Category;
using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface ICategoryApplicationServiceContract
    {
        Task<GenericOperationResult<List<CategoryUpdateModel>>> Get(int Categoryid);
        Task<OperationResult> AddNew(CategoryAddModel model);
        Task<OperationResult> Remove(int id);
        Task<OperationResult> Update(CategoryUpdateModel model);
        Task<GenericComplexResult<CategorySearchModel, CategoryListItem>> Search(CategorySearchModel sm);
        Task<OperationResult> AssignFeatureToCategory(int categoryId, int FeaturId);
        Task<OperationResult> RemoveFeatureToCategory(int categoryId, int FeaturId);
        
    }
}
