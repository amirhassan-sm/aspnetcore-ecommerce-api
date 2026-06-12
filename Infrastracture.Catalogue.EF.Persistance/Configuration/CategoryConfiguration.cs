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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.Property(x=>x.CategoryId).IsRequired();
            builder.Property(x=>x.CategoryName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CategoryDescription).IsRequired(false).HasMaxLength(4000);
            builder.Property(x => x.MetaData).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.MetaTag).IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.LineAge).IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.Slug).IsRequired(false).HasMaxLength(500);
            builder.Property(x=>x.ParentId).HasDefaultValue(null);

            builder
                .HasMany(x => x.Products)
                .WithOne(x => x.category).HasForeignKey(x=>x.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.CategoryFeatures).WithOne(x => x.category).HasForeignKey(x=>x.CategoryId);
            builder.HasMany(x=>x.Children).WithOne(x=>x.Parent).HasForeignKey(x=>x.ParentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.HasIndex(x => x.CategoryName).IsUnique();








        }
    }
}
