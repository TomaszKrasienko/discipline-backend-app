using discipline.application.Infrastructure.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.api.integration_tests._Helpers;

public abstract class BaseTestsController : IDisposable
{
   private readonly TestAppDb _testAppDb;
   protected readonly HttpClient HttpClient;
   internal readonly DisciplineDbContext DbContext;
   protected BaseTestsController()
   {
       var app = new TestApp(ConfigureServices);
       _testAppDb = new TestAppDb();
       DbContext = _testAppDb.DisciplineDbContext;
       HttpClient = app.HttpClient;
       
   }

   protected virtual void ConfigureServices(IServiceCollection services)
   {
   }
   
   public void Dispose()
       => _testAppDb.Dispose();
}