using discipline.application.Infrastructure.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.api.integration_tests._Helpers;

internal abstract class BaseTestsController : IDisposable
{
    private readonly TestAppDb _testAppDb;
    protected readonly HttpClient HttpClient;
    protected readonly DisciplineDbContext DbContext;
    
    protected BaseTestsController()
    {
        var app = new TestApp(ConfigureServices);
        _testAppDb = new TestAppDb();
        HttpClient = app.HttpClient;
        DbContext = _testAppDb.DisciplineDbContext;
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    public void Dispose()
        => _testAppDb.Dispose();
}