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
            return await db.productMedias.FirstOrDefaultAsync(x=>x.ProductMediaId == id);

        }

        public async Task<List<ProductMedia>> GetByProductIdAsync(int productId)
        {
            return await db.productMedias.Where(x=>x.ProductId == productId).OrderBy(x => x.DisplayOrder).ToListAsync();
        }

        public async Task<List<ProductMedia>> GetImagesByProductIdAsync(int productId)
        {
            var imageMedia = await db.mediaTypes.FirstOrDefaultAsync(x => x.Name == "Image");
            if (imageMedia == null)
            {
                return new List<ProductMedia>();
            }

            return await db.productMedias
                .Where(x => x.ProductId == productId && x.MediaTypeId == imageMedia.MediaTypeId)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<ProductMedia>> GetVideosByProductIdAsync(int productId)
        {
            var videoMedia = await db.mediaTypes.FirstOrDefaultAsync(x => x.Name == "Video");
            if (videoMedia == null)
            {
                return new List<ProductMedia>();
            }

            return await db.productMedias
                .Where(x => x.ProductId == productId && x.MediaTypeId == videoMedia.MediaTypeId)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<string> GetMediaType(int MediaId)
        {
            var type = await db.mediaTypes.FirstOrDefaultAsync(x => x.MediaTypeId == MediaId);
            return type?.Name ?? string.Empty;
        }

        public async Task<int> GetProductMediaDisplay(int productId,int mediaType)
        {
            var maxOrder = await db.productMedias
      .Where(x => x.ProductId == productId && x.MediaTypeId == mediaType)
      .Select(x => (int?)x.DisplayOrder)
      .MaxAsync();
            return (maxOrder ?? 0) + 1;
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
