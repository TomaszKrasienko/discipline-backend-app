using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Connection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace discipline.api.integration_tests._Helpers;

public abstract class BaseTestsController : IDisposable
{
   internal readonly TestAppDb TestAppDb;
   protected readonly HttpClient HttpClient;
   protected BaseTestsController()
   {
       var app = new TestApp(ConfigureServices);
       TestAppDb = new TestAppDb();
       HttpClient = app.HttpClient;
       
   }

   protected virtual void ConfigureServices(IServiceCollection services)
   {
       services.AddSingleton<IMongoCollectionNameConvention, TestsMongoCollectionNameConvention>();
   }
   
   protected virtual Guid? GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
   {
       if (httpResponseMessage is null)
       {
           throw new InvalidOperationException("Http response message is null");
       }

       if (!httpResponseMessage.Headers.TryGetValues(AddingResourceIdHeaderBehaviour.HeaderName, out var value))
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

   protected virtual MetaDataDto GetMetaDataFromHeader(HttpResponseMessage httpResponseMessage)
   {
       if (httpResponseMessage is null)
       {
           throw new InvalidOperationException("Http response message is null");
       }

       if (!httpResponseMessage.Headers.TryGetValues(PagingBehaviour.HeaderName, out var value))
       {
           return null;
       }
       
       var metaDataDto = value.Single();
       return JsonConvert.DeserializeObject<MetaDataDto>(metaDataDto);
   }
   
   public void Dispose()
       => TestAppDb.Dispose();
}