using Application.Common.BaseModel;
using Application.DTOs;
using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contrast.Services
{
    public interface IProductMediaService
    {
        Task<OperationResult> AddMediaAsync(ProductMediaModel media);

        Task<GenericOperationResult<ProductMediaModel>> GetByIdAsync(int id);

        Task<GenericOperationResult<List<ProductMediaModel>>> GetByProductIdAsync(int productId);

        Task<OperationResult> RemoveAsync(int id);
    }
}
