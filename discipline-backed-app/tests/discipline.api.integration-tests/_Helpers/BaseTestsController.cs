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
   
   protected virtual Guid? GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
   {
       if (httpResponseMessage is null)
       {
           throw new InvalidOperationException("Http response message is null");
       }

       if (!httpResponseMessage.Headers.TryGetValues("x-resource-id", out var value))
       {
           return null;
       }

       var stringId = value.Single();
       if (!Guid.TryParse(stringId, out var id))
       {
           throw new InvalidOperationException("Resource id is not GUID type");
       }

       return id;
   }
   
   public void Dispose()
       => _testAppDb.Dispose();
}