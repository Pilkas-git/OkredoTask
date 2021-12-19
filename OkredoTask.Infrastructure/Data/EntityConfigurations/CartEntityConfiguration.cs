using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.CartId);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}