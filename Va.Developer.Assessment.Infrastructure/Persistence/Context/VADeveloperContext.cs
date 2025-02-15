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
        public override EntityEntry Add(object entity)
        {
            if(entity is Transaction model){ 
                model.CaptureDate = DateTime.Now;
            }
            return base.Add(entity);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureRelations();
        }
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if(entity is Transaction model){ 
                model.CaptureDate = DateTime.Now;
            }
            return base.Add(entity);
        }
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if(entity is Transaction model){ 
                model.CaptureDate = DateTime.Now;
            }   
            return base.Update(entity);
        }
        public override EntityEntry Update(object entity)
        {
            if(entity is Transaction model){ 
                model.CaptureDate = DateTime.Now;
            }
            return base.Update(entity);
        }
    }
}