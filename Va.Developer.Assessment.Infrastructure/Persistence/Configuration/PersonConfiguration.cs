
namespace Va.Developer.Assessment.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Va.Developer.Assessment.Domain.Models;

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Code);

            builder
                .Property(p => p.IdNumber)
                .HasMaxLength(50)
                .IsRequired();
             builder
                .Property(p => p.Name)
                .HasMaxLength(50);
            builder
                .Property(p => p.Surname)
                .HasMaxLength(50);
            builder
                .HasIndex(p => p.Code)
                .HasDatabaseName("IX_Person_id")
                .IsClustered(false)
                .IsUnique();
            
            builder
                .HasMany(p => p.Accounts)
                .WithOne()
                .HasConstraintName("FK_Account_Person")
                .HasForeignKey(a => a.PersonCode)
                .IsRequired();

            builder.ToTable("Persons");
        }
    }
}