namespace Va.Developer.Assessment.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Va.Developer.Assessment.Domain.Models;

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Code);

            builder
                .Property(t => t.TransactionDate)
                .IsRequired();
            builder
                .Property(t => t.CaptureDate)
                .IsRequired();
            builder
                .Property(t => t.AccountCode)
                .IsRequired();
             builder
                .Property(t => t.Description)
                .HasMaxLength(100)
                .IsRequired();
            builder.ToTable("Transactions", t => {
                t.HasCheckConstraint
                (name: "CK_Transaction_Amount_NotZero",  
                 sql: "[Amount] > 0");
            });
        }
    }
}