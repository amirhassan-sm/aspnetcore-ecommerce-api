using Application.Common.BaseModel;
using Application.DTOs.ProducMedia;
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

        Task<GenericOperationResult<List<ProductMediaModel>>> GetAllMediaByProductIdAsync(int productId);
        Task<GenericOperationResult<List<ProductMediaModel>>> GetImagesByProductIdAsync(int productId);

        Task<GenericOperationResult<List<ProductMediaModel>>> GetVideosByProductIdAsync(int productId);

        Task<OperationResult> RemoveAsync(int id);
    }
}
