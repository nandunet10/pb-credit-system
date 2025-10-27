using Microsoft.EntityFrameworkCore;
using PB.CreditProposalService.Domain.Entities;

namespace PB.CreditProposalService.Infrastructure.Data
{
    public class ProposalDbContext : DbContext
    {
        public ProposalDbContext(DbContextOptions<ProposalDbContext> options)
            : base(options)
        {
        }

        public DbSet<CreditProposal> CreditProposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CreditProposal>(entity =>
            {
                entity.ToTable("CreditProposals");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.Score).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>().IsRequired();
                entity.Property(e => e.ApprovedCards).IsRequired();
                entity.Property(e => e.CreditLimitPerCard).HasPrecision(18, 2);
                entity.Property(e => e.RejectionReason).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasIndex(e => e.CustomerId);
            });
        }
    }
}
