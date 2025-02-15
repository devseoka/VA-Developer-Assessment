using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Va.Developer.Assessment.Domain.Models;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Code);
            builder
                .HasIndex(a => a.AccountNumber)
                .HasDatabaseName("IX_Account_num")
                .IsUnique()
                .IsClustered(false);

            builder
                .Property(a => a.Balance)
                .HasMaxLength(50)
                .IsRequired();
            builder
                .HasMany(a => a.Transactions)
                .WithOne()
                .HasConstraintName(" FK_Transaction_Account")
                .HasForeignKey(t => t.AccountCode)
                .IsRequired();

        }
    }
}