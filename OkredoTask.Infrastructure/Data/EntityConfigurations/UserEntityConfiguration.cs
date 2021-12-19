using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OkredoTask.Core.Entities;

namespace OkredoTask.Infrastructure.Data.Config
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Email)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(16);

            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => x.DeletedOn == null);
        }
    }
}