using Catalogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.DomainServiceContract.Service
{
    public interface IProductMediaRepository
    {
        Task AddAsync(ProductMedia media);

        Task<ProductMedia?> GetByIdAsync(int id);

        Task<List<ProductMedia>> GetByProductIdAsync(int productId);

        Task RemoveAsync(int id);

        Task<bool> IsMediaTypeExist(int mediaTypeId);
        Task<bool> IsProductMediaExist(int productMediaId);
        Task<string> GetMediaType(int MediaId);

        Task SaveAsync();
    }
}
