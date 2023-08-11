using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Services.AuthAPI.Models;

namespace Restaurant.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers  { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Name = "CUSTOMER"
            });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Name = "ADMIN"
            });
        }
    }
}
