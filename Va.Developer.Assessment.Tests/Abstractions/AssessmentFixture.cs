namespace Va.Developer.Assessment.Tests.Abstractions
{
    using DotNet.Testcontainers.Builders;
    using Microsoft.EntityFrameworkCore;
    using Testcontainers.MsSql;
    public class AssessmentFixture : IAsyncLifetime
    {
        private VaDeveloperContext _context;
        private static readonly Lazy<MsSqlContainer> _lazyLoadContainer =
            new(() => new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPassword("P@ssW0rd1").WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1434))
            .WithCleanUp(true)
            .Build());
        private static MsSqlContainer SqlContainer => _lazyLoadContainer.Value;
        public string ConnectionString => SqlContainer.GetConnectionString();
        public VaDeveloperContext Context => _context;
        public async Task DisposeAsync()
        {
            await SqlContainer.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            await SqlContainer.StartAsync();
            var builder = new DbContextOptionsBuilder<VaDeveloperContext>()
                .UseSqlServer(connectionString: ConnectionString);
            _context = new VaDeveloperContext(builder.Options);

          
        }
    }
}
