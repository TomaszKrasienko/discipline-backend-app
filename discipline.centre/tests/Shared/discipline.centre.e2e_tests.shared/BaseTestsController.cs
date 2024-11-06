using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.e2e_tests.shared;

public abstract class BaseTestsController : IDisposable
{
    public TestAppDb TestAppDb { get; set; }
    protected HttpClient HttpClient { get; set; }

    protected BaseTestsController(string moduleName)
    {
        var app = new TestApp(ConfigureServices);
        TestAppDb = new TestAppDb(moduleName);
        HttpClient = app.HttpClient;
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoCollectionNameConvention, TestsMongoCollectionNameConvention>();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
   
    protected virtual void Dispose(bool disposed)
    { 
        TestAppDb?.Dispose();
        HttpClient?.Dispose();
    }
}