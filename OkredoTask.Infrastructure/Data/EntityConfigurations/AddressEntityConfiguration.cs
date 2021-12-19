using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class AddressEntityConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.Street)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.PostalCode)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}