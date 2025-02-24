using Microsoft.Extensions.DependencyInjection.Extensions;
using Va.Developer.Assessment.Domain.Contracts.Repository;
using Va.Developer.Assessment.Infrastructure.Persistence.Repositories;

namespace Va.Developer.Assessment.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, string connection){
       services.AddDbContext<VaDeveloperContext>((serviceProvider, options) => {
            options.UseSqlServer( connectionString: connection);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        return services;
    }
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.TryAddScoped<IPersonRepostiory, PersonRepository>();
        services.TryAddScoped<IAccountRepository, AccountRepository>();
        services.TryAddScoped<ITransactionRepository,TransactionRepository>();
        return services;
    }
}