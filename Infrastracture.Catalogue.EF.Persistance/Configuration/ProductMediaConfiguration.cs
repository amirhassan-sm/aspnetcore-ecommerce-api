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
    public class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
    {
        public void Configure(EntityTypeBuilder<ProductMedia> builder)
        {
           builder.HasKey(x=>x.ProductMediaId);
          
            builder.Property(x=>x.IsConfirmed).IsRequired(false);
            builder.Property(x=>x.ConfirmByUserName).IsRequired(false);
            builder.Property(x=>x.fileSize).IsRequired();

            builder.HasOne(x=>x.Product).WithMany(x=>x.productMedia).HasForeignKey(x=>x.ProductId);
            builder.HasOne(x=>x.MediaType).WithMany(x=>x.ProductMedia).HasForeignKey(x=>x.MediaTypeId);
        }
    }
}
