using Microsoft.EntityFrameworkCore;
using PB.CreditCardService.Domain.Entities;

namespace PB.CreditCardService.Infrastructure.Data
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> options) : base(options) { }

        public DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.ToTable("CreditCards");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProposalId).IsRequired();
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.CardNumber).IsRequired().HasMaxLength(16);
                entity.Property(e => e.CVV).IsRequired().HasMaxLength(3);
                entity.Property(e => e.ExpirationDate).IsRequired();
                entity.Property(e => e.CreditLimit).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.AvailableLimit).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>().IsRequired();
                entity.Property(e => e.IssuedAt).IsRequired();

                entity.HasIndex(e => e.CustomerId);
                entity.HasIndex(e => e.ProposalId);
                entity.HasIndex(e => e.CardNumber).IsUnique();
            });
        }
    }
}
