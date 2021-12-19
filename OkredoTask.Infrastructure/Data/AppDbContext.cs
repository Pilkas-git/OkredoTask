using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OkredoTask.Core.Entities;
using OkredoTask.Infrastructure.Data.Config;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OkredoTask.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            HandlePersistentEntity();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            HandlePersistentEntity();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new AddressEntityConfiguration().Configure(builder.Entity<Address>());
            new CartEntityConfiguration().Configure(builder.Entity<Cart>());
            new CartItemEntityConfiguration().Configure(builder.Entity<CartItem>());
            new DiscountEntityConfiguration().Configure(builder.Entity<Discount>());
            new OrderEntityConfiguration().Configure(builder.Entity<Order>());
            new OrderItemEntityConfiguration().Configure(builder.Entity<OrderItem>());
            new ProductEntityConfiguration().Configure(builder.Entity<Product>());
            new SupplierEntityConfiguration().Configure(builder.Entity<Supplier>());
            new UserEntityConfiguration().Configure(builder.Entity<User>());
        }

        private void HandlePersistentEntity()
        {
            ChangeTracker.DetectChanges();
            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            var markedAsUpdated = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            var markedAsCreated = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var item in markedAsDeleted.Where(x => x.Entity is PersistentEntityBase))
            {
                if (item.Entity is PersistentEntityBase)
                {
                    item.State = EntityState.Unchanged;
                    item.CurrentValues[nameof(PersistentEntityBase.DeletedOn)] = DateTime.Now;
                }
            }

            foreach (var item in markedAsUpdated)
            {
                if (item.Entity is PersistentEntityBase)
                {
                    item.CurrentValues[nameof(PersistentEntityBase.ModifiedOn)] = DateTime.Now;
                }
            }

            foreach (var item in markedAsCreated)
            {
                if (item.Entity is PersistentEntityBase)
                {
                    item.CurrentValues[nameof(PersistentEntityBase.CreatedOn)] = DateTime.Now;
                }
            }
        }
    }
}