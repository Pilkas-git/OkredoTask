using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class SupplierEntityConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.SupplierId);

            builder.Property(x => x.SupplierName)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasMany(x => x.Products)
               .WithOne(x => x.Supplier)
               .HasForeignKey(x => x.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}