using Catalogue.Domain.Models;
using Infrastracture.Catalogue.EF.Persistance.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Catalogue.EF.Persistance
{
    public class CatalogueContext:DbContext
    {
        public CatalogueContext(DbContextOptions<CatalogueContext> options):base(options)
        {
            
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Feature> features { get; set; }
        public DbSet<CategoryFeature> CategoryFeatures { get; set; }
        public DbSet<Product> products{ get; set; }
        public DbSet<ProductFeature> ProductFeatures{ get; set; }
        public DbSet<ProductMedia> productMedias { get; set; }
        public DbSet<MediaType> mediaTypes{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Category>(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());
            modelBuilder.ApplyConfiguration<Feature>(new  FeatureConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
