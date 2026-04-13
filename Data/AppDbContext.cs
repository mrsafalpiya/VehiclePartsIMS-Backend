using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using VehiclePartsIMS_Backend.Data.Entities;

namespace VehiclePartsIMS_Backend.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<PartRequest> PartRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

            SeedRoles(builder);
            SeedAdmin(builder);
        }

        public void SeedRoles(ModelBuilder builder)
        {
            List<IdentityRole<int>> roles = [
                new IdentityRole<int> {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "133257ba-cf42-4cd4-a326-5b513272e9de"
                },
                new IdentityRole<int> {
                    Id = 2,
                    Name = "Staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = "7485b776-cf12-45ca-9a6a-40aa8b58c98b",
                },
                new IdentityRole<int> {
                    Id = 3,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "797dd241-a38a-4a0e-867c-75b944a37300",
                },
            ];

            builder.Entity<IdentityRole<int>>().HasData(roles);
        }

        public void SeedAdmin(ModelBuilder builder)
        {
            var admin = new User
            {
                Id = 1,
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEHJuboYiX8TZciAU5na/HqkcTjqvg9XQKlbwSQIpIW/CXrocigNl+wuxwMlzigP7oQ==", // admin@123
                SecurityStamp = string.Empty,
                ConcurrencyStamp = "133257ba-cf42-4cd4-a326-5b513272e9de",
                FullName = "System Admin"
            };

            builder.Entity<User>().HasData(admin);
        }
    }
}
