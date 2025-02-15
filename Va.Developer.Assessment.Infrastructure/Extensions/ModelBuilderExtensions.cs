namespace Va.Developer.Assessment.Infrastructure.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Va.Developer.Assessment.Infrastructure.Persistence.Configuration;

    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ConfigureRelations(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PersonConfiguration());
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            return builder;
        }
    }
}