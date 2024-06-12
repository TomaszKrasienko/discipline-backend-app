using discipline.application.Infrastructure.DAL;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using Microsoft.EntityFrameworkCore;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestAppDb : IDisposable
{
    private const string SectionName = "Postgres";
    internal DisciplineDbContext DisciplineDbContext { get; set; }

    public TestAppDb()
    {
        var options = new OptionsProvider().Get<PostgresOptions>(SectionName);
        var contextOptions = new DbContextOptionsBuilder<DisciplineDbContext>()
            .UseNpgsql(options.ConnectionString)
            .EnableSensitiveDataLogging()
            .Options;
        DisciplineDbContext = new DisciplineDbContext(contextOptions);
    }
    
    public void Dispose()
    {
        DisciplineDbContext.Database.EnsureDeleted();
    }
}