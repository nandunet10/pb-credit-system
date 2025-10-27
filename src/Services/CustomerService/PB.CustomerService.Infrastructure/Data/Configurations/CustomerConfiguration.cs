using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PB.CustomerService.Domain.Entities;

namespace PB.CustomerService.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(c => c.CPF)
                .IsUnique();

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Income)
                .HasPrecision(18, 2);

            builder.Property(c => c.Status)
                .HasConversion<string>();

            // Owned Entity - Address
            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("AddressStreet");

                address.Property(a => a.Number)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("AddressNumber");

                address.Property(a => a.Complement)
                    .HasMaxLength(100)
                    .HasColumnName("AddressComplement");

                address.Property(a => a.Neighborhood)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("AddressNeighborhood");

                address.Property(a => a.City)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("AddressCity");

                address.Property(a => a.State)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("AddressState");

                address.Property(a => a.ZipCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("AddressZipCode");
            });

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);
        }
    }
}
