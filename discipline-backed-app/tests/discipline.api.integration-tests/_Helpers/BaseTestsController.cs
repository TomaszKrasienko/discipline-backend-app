using System.Text.Json.Serialization;
using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
       => _testAppDb.Dispose();
}