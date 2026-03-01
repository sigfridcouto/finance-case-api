using FinanceCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FinanceCase.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<FinanceCase.Domain.Entities.FinanceCase> FinanceCases => Set<FinanceCase.Domain.Entities.FinanceCase>();
    public DbSet<CaseNote> CaseNotes => Set<CaseNote>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(b =>
        {
            b.ToTable("Customers");
            b.HasKey(x => x.Id);
            b.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200);
        });

        modelBuilder.Entity<FinanceCase.Domain.Entities.FinanceCase>(b =>
        {
            b.ToTable("FinanceCases");
            b.HasKey(x => x.Id);

            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.RequestedAmount).HasColumnType("decimal(18,2)");
            b.Property(x => x.AnnualIncome).HasColumnType("decimal(18,2)");

            b.HasOne(x => x.Customer)
             .WithMany()
             .HasForeignKey(x => x.CustomerId);

            // Notes field mapping (backing field)
            b.Metadata.FindNavigation(nameof(FinanceCase.Domain.Entities.FinanceCase.Notes))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            b.HasMany<CaseNote>("_notes")
             .WithOne()
             .HasForeignKey(n => n.FinanceCaseId);
        });

        modelBuilder.Entity<CaseNote>(b =>
        {
            b.ToTable("CaseNotes");
            b.HasKey(x => x.Id);
            b.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        });
    }
}