using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Va.Developer.Assessment.Tests.Abstractions
{
    public class WebAssessmentFactory : WebApplicationFactory<Program>
    {
        public AssessmentFixture Fixture { get; }
        public IPersonRepostiory PersonRepostiory { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IAccountRepository AccountRepository { get; }
        public IPersonService PersonService { get; }
        public IValidator<AccountDto> AccountValidator { get; }
        public IMapper Mapper { get; }

        private readonly ILogger<WebAssessmentFactory> _logger;

        
        public WebAssessmentFactory(AssessmentFixture fixture)
        {
            Fixture = fixture;
            var scope = Services.CreateScope();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<WebAssessmentFactory>>();
            PersonRepostiory = scope.ServiceProvider.GetRequiredService<IPersonRepostiory>();
            AccountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
            TransactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
            Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            AccountValidator = scope.ServiceProvider.GetRequiredService<IValidator<AccountDto>>();
            PersonService = scope.ServiceProvider.GetRequiredService<IPersonService>();

        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((services) =>
            {
                var type = typeof(DbContextOptions<VaDeveloperContext>);
                var context = services.Single(descriptor => descriptor.ServiceType == type);
                services.Remove(context);
                var ctx = services.Single(d => d.ServiceType == typeof(VaDeveloperContext));
                services.Remove(ctx!);
                services.AddDbContext<VaDeveloperContext>((opts) =>
                {
                    opts
                    .UseSqlServer(Fixture.ConnectionString)
                    .EnableSensitiveDataLogging();
                });

            });
        }
        public  async Task ExecuteSqlScripts(string tableName)
        {
            try
            {
                if (!await DoesTableExistAsync(tableName))
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "query.sql");
                    if (!File.Exists(path))
                    {
                        _logger.LogError("{File} does not exists in {Directory}", Path.GetFileName(path), Path.GetDirectoryName(path));
                        return;
                    }
                    string sql = await File.ReadAllTextAsync(path);
                    string[] sqlStatements = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (string sqlStatement in sqlStatements)
                    {
                        if (string.IsNullOrWhiteSpace(sqlStatement))
                        {
                            continue;
                        }
                        await Fixture.Context.Database.ExecuteSqlRawAsync(sqlStatement);
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex,"An unexpected error occured while setting up base data. {Message}", ex);
                throw;
            }
        }
        public async Task<bool> DoesTableExistAsync(string tableName)
        {
            using var connection = new SqlConnection(Fixture.Context.Database.GetDbConnection().ConnectionString);
            await connection.OpenAsync();

            string sql = @"
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = @tableName
            ) THEN 1 ELSE 0 END";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@tableName", tableName);
            var result = (int)await command.ExecuteScalarAsync();
            return result == 1;
        }


    }
}
