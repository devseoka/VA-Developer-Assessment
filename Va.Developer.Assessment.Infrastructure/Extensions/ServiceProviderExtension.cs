
namespace Va.Developer.Assessment.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task<IServiceProvider> CreateSchema(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<VaDeveloperContext>();
        context.Database.Migrate();
        context.Database.ExecuteSqlRaw(@"IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'audit')
			BEGIN EXEC('CREATE SCHEMA audit'); END");

        return serviceProvider;
    }
}