using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.Name)
               .HasMaxLength(128)
               .IsRequired();

            builder.Property(x => x.Price)
               .HasColumnType("decimal(18,4)")
               .IsRequired();

            builder.Property(x => x.Description)
               .HasMaxLength(2048)
               .IsRequired();

            builder.Property(x => x.AvailabilityCount)
               .IsConcurrencyToken()
               .HasDefaultValue(0);

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}