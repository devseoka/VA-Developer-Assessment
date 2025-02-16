namespace Va.Developer.Assessment.Application.Extensions
{
    public static class ServiceCollections
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
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
                });
            });
        }
    }
}