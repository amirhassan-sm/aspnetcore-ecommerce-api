using Infrastructure.Sale.Configuration;
using Microsoft.EntityFrameworkCore;
using Sale.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sale
{
    public class SaleSystemContext:DbContext
    {
        public SaleSystemContext(DbContextOptions<SaleSystemContext>options )
            : base(options)
        {

            
        }
        public DbSet<OrderDetails> OrderDetail { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SaleSystemContext).Assembly);
            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(x => x.OrderDetailId);
                entity.ToTable("OrderDetails");
            });
            base.OnModelCreating(modelBuilder);
        }




    }
}
