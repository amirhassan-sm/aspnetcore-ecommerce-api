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
    public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.Property(x=>x.FeatureName).IsRequired().HasMaxLength(256);
            builder.HasIndex(x => x.FeatureName).IsUnique();
            
            builder.Property(x=>x.FeatureDescription).IsRequired(false).HasMaxLength(1500);

        }
    }
}
