using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistanse.Identity
{
    public class IdentityContext(DbContextOptions<IdentityContext> options):IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityRoleClaim<string>>();
            modelBuilder.Entity<IdentityUserClaim<string>>();
            modelBuilder.Entity<IdentityUserLogin<string>>();
            modelBuilder.Entity<IdentityUserToken<string>>();
        }
    }
}
