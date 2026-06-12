
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class SecurityContext: IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public SecurityContext(DbContextOptions<SecurityContext> option):base(option)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppliactionUserRole>().HasKey(x => new
            {
                x.ApplicationRoleId,
                x.ApplicationUserId,

            });
            builder.Entity<ApplicationRole>().HasKey(x => x.ApplicationUserRoleID);
            builder.Entity<ApplicationRole>().Property(x=>x.Description).HasMaxLength(50);
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.appliactionUserRoles).WithOne(x => x.applicationUser).HasForeignKey(x=>x.ApplicationUserId);
            builder.Entity<ApplicationRole>()
                .HasMany(x => x.appliactionUserRoles).WithOne(x => x.applicationRole).HasForeignKey(x => x.ApplicationRoleId);
            base.OnModelCreating(builder);
        }

    }
}
