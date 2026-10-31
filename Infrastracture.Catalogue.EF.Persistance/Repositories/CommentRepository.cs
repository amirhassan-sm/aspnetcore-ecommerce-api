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
    public class CommentRepository : ICommentRepository
    {
        private readonly CatalogueContext db;
        public CommentRepository(CatalogueContext db)
        {
            this.db = db;
            
        }
        public async Task<int> AddNew(Comment model)
        {
            model.CommentDate = DateTime.UtcNow;
             db.comments.Add(model);
            await db.SaveChangesAsync();
            return model.CommentId;
        }

        public async Task<Comment?> Get(int id)
        {
            return await db.comments.FindAsync(id);
        }

        public async Task<IReadOnlyList<Comment>> GetCommentsByProductIdAsync(int productId)
        {
            return await db.comments.AsNoTracking().Where(x=>x.ProductId == productId).ToListAsync();
        }

        public async Task<bool> Remove(int id)
        {
            var entity = await db.comments.FirstOrDefaultAsync(x=>x.CommentId == id);
            if (entity == null)
            {
                return false;
                
            }
            db.comments.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(Comment model)
        {
            var entity = await db.comments.FirstOrDefaultAsync(x=>x.CommentId== model.CommentId);
            if (entity == null)
            {
                return false;

            }
            entity.Content = model.Content;
            entity.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return true;

        }
    }
}
