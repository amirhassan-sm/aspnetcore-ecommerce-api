using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sale.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sale.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.OrderId).IsRequired();
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.CustomerName).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.Province).IsRequired();
            builder.Property(x=>x.City).IsRequired(false);
            builder.Property(x=>x.Street).IsRequired(false);


            builder.HasMany(x => x.orderDetails).WithOne(x => x.order).HasForeignKey(x => x.OrderId);
        }
    }
}
