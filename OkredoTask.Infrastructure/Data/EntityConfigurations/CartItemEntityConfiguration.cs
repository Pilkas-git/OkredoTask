using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => new { x.CartId, x.ProductId });

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.HasOne(x => x.Cart)
                .WithMany(x => x.CartItems)
                .HasPrincipalKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.Cart.DeletedOn == null);
            builder.HasQueryFilter(x => x.Product.DeletedOn == null);
        }
    }
}