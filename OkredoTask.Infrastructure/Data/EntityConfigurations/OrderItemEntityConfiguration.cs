using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasPrincipalKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.Order.User.DeletedOn == null);
        }
    }
}