using Catalogue.Domain.Models;
using Catalogue.DomainServiceContract.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Catalogue.EF.Persistance.Repositories
{
    public class ProductMediaRepository : IProductMediaRepository
    {
        private readonly CatalogueContext db;
        public ProductMediaRepository(CatalogueContext db)
        {
            this.db = db;

            
        }
        public async Task AddAsync(ProductMedia media)
        {
           db.productMedias.Add(media);
           await db.SaveChangesAsync();
        }

        public async Task<ProductMedia?> GetByIdAsync(int id)
        {
            return await db.productMedias.FirstOrDefaultAsync(x=>x.MediaTypeId == id);

        }

        public async Task<List<ProductMedia>> GetByProductIdAsync(int productId)
        {
            return await db.productMedias.Where(x=>x.ProductId == productId).ToListAsync();
        }

        public async Task<string> GetMediaType(int MediaId)
        {
            var type= await db.mediaTypes.FirstOrDefaultAsync(x => x.MediaTypeId==MediaId);
            var name = type.Name;
            return name;

        }

        public async Task<bool> IsMediaTypeExist(int mediaTypeId)
        {
           return await db.mediaTypes.AnyAsync(x=>x.MediaTypeId==mediaTypeId);
        }

        public async Task<bool> IsProductMediaExist(int productMediaId)
        {
            return await  db.productMedias.AnyAsync(x=>x.ProductMediaId == productMediaId);
        }

        public async Task RemoveAsync(int id)
        {
             var media = db.productMedias.FirstOrDefault(x=>x.ProductMediaId == id);
            db.productMedias.Remove(media);
            await db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
             await db.SaveChangesAsync();
            
        }
    }
}
