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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(300);
            builder.Property(x => x.ProductDescription).IsRequired(false).HasMaxLength(1500);
            builder.Property(x => x.ImageUrl).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.Introduction).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.ExtraData).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.BodyExtraData).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.Slug).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.MetaTag).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.MetaDescription).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.PageTitle).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.BasePrice).IsRequired().HasMaxLength(1000);



        }
    }
}
