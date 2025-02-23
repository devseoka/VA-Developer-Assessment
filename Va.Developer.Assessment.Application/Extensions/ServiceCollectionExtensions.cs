namespace Va.Developer.Assessment.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAutoMapperAndValidators(this IServiceCollection services)
        {
            return services
                        .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                        .AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.TryAddScoped<IPersonService, PersonService>();
            services.TryAddScoped<IAccountService, AccountService>();
            services.TryAddScoped<ITransactionManager, TransactionManager>();
            services.TryAddScoped<ITransactionService, TransactionService>();
            return services;
        }
        public static IServiceCollection EnableCors(this IServiceCollection services, string name)
        {
            return services.AddCors((options) =>
            {
                options.AddPolicy(name, opts =>
                {
                    opts
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}