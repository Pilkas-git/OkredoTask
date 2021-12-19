using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OkredoTask.Core.Constants;
using OkredoTask.Core.Entities;
using OkredoTask.Core.Enums;
using OkredoTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            var adminUserOptions = serviceProvider.GetService<IOptions<AdminUserOptions>>().Value;

            Task.Run(() => SeedDatabaseAsync(dbContext, adminUserOptions)).Wait();
        }

        public static async Task SeedDatabaseAsync(AppDbContext dbContext, AdminUserOptions adminUserOptions)
        {
            //TODO Remove suppliers, products seeds when sync service with suppliers implemented
            await SeedSuppliersAsync(dbContext);
            await SeedProductsAsync(dbContext);

            await SeedRolesAsync(dbContext);
            await SeedUsersAsync(dbContext, adminUserOptions);
            await CreateFullTextCatalogAsync(dbContext);
        }

        private static async Task SeedSuppliersAsync(AppDbContext dbContext)
        {
            if (dbContext.Suppliers.Any())
            {
                return;   // DB has been seeded
            }

            var suppliers = new List<Supplier>
            {
                new Supplier("ElektroShop"),
                new Supplier("Mobil")
            };

            dbContext.Suppliers.AddRange(suppliers);

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(AppDbContext dbContext)
        {
            if (dbContext.Products.Any())
            {
                return;   // DB has been seeded
            }

            var supplier = await dbContext.Suppliers.FirstOrDefaultAsync();

            var products = new List<Product>
            {
                new Product(supplier.SupplierId,"Acer traverMate", 900.99m, "Description",
                    ProductType.Laptop),
                new Product(supplier.SupplierId,"Lenovo ThinkBook", 522.99m, "Description",
                    ProductType.Laptop),
                new Product(supplier.SupplierId,"Iphone X", 190.99m, "Description",
                    ProductType.Phone),
                new Product(supplier.SupplierId,"Samsung Galaxy s4", 130.99m, "Description",
                    ProductType.Phone),
                new Product(supplier.SupplierId,"Asus ProArt ", 400.99m, "Description",
                    ProductType.Monitor),
                new Product(supplier.SupplierId,"Dell S2521", 290.99m, "Description",
                    ProductType.Monitor),
            };

            products.ForEach(p => p.SetAvailabilityCount(5));

            dbContext.Products.AddRange(products);

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(AppDbContext dbContext, AdminUserOptions adminUserOptions)
        {
            if (dbContext.ApplicationUsers.Any())
            {
                return;   // DB has been seeded
            }

            var ownerRoleId = dbContext.Roles.FirstOrDefault(a => a.Name.Equals(RoleConstants.Admin)).Id;

            var userIdentity = new IdentityUser("user@mail.com") { Email = "user@mail.com", NormalizedEmail = "USER@MAIL.COM" };
            var adminIdentity = new IdentityUser(adminUserOptions.AdminUsername)
            { Email = adminUserOptions.AdminUsername, NormalizedEmail = adminUserOptions.AdminUsername.ToUpperInvariant() };

            var ph = new PasswordHasher<IdentityUser>();
            adminIdentity.PasswordHash = ph.HashPassword(adminIdentity, adminUserOptions.AdminPass);
            userIdentity.PasswordHash = ph.HashPassword(userIdentity, "user");
            dbContext.Users.Add(userIdentity);
            dbContext.Users.Add(adminIdentity);
            await dbContext.SaveChangesAsync();

            dbContext.UserRoles.Add(new IdentityUserRole<string>() { UserId = adminIdentity.Id, RoleId = ownerRoleId.ToString() });
            var user = new User("user", "user", "user@mail.com");
            var adminAppUser = new User("admin", "admin", adminUserOptions.AdminUsername);

            adminAppUser.SetUserRole(UserRole.Administrator);

            dbContext.ApplicationUsers.Add(user);
            dbContext.ApplicationUsers.Add(adminAppUser);

            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(AppDbContext dbContext)
        {
            if (dbContext.Roles.Any())
            {
                return;   // DB has been seeded
            }

            dbContext.Roles.Add(new IdentityRole
            {
                Name = RoleConstants.Admin,
                NormalizedName = RoleConstants.Admin.ToUpperInvariant()
            });

            await dbContext.SaveChangesAsync();
        }

        private static async Task CreateFullTextCatalogAsync(AppDbContext dbContext)
        {
            const string sql = "IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE[name] = 'Search')" +
                " BEGIN" +
                "   CREATE FULLTEXT CATALOG Search WITH ACCENT_SENSITIVITY = OFF" +
                "   CREATE FULLTEXT INDEX ON dbo.Products(name) KEY INDEX PK_Products ON Search" +
                " END";
            await dbContext.Database.ExecuteSqlRawAsync(sql);

            await dbContext.SaveChangesAsync();
        }
    }
}