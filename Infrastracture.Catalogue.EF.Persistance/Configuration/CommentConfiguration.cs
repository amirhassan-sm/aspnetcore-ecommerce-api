using Catalogue.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Catalogue.EF.Persistance.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.CommentId).IsRequired();
            builder.Property(x=>x.Content).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.ParentCommentId).IsRequired(false);
            builder.HasMany(x=>x.Replies).WithOne(x=>x.ParentComment).HasForeignKey(x=>x.ParentCommentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.Product).WithMany(x=>x.comments).HasForeignKey(x=>x.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
