using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Va.Developer.Assessment.Domain.Models;
using Va.Developer.Assessment.Infrastructure.Extensions;

namespace Va.Developer.Assessment.Infrastructure.Persistence.Context
{
    public class VaDeveloperContext(DbContextOptions<VaDeveloperContext> options) : DbContext(options)
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureRelations();
        }
    }
}