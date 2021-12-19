using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class DiscountEntityConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(x => x.DiscountId);

            builder.Property(x => x.DiscountCode)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.DiscountValue)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.Property(x => x.IsSingleUse)
                .IsRequired();

            builder.Property(x => x.IsValid)
                .IsRequired();

            builder.Property(x => x.Expires)
                .IsRequired();

            builder.Property(x => x.IsFixedValue)
                .IsRequired();

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}