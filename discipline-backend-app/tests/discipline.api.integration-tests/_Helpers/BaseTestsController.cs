using System.Net.Http.Headers;
using Amazon.Runtime;
using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.Users.Entities;
using discipline.tests.shared.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
   
   protected async Task<User> AuthorizeWithFreeSubscriptionPicked()
   {
       var subscription = SubscriptionFactory.Get();
       var user = UserFactory.Get();
       user.CreateFreeSubscriptionOrder(Guid.NewGuid(), subscription, DateTime.Now);
       await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
       Authorize(user.Id, user.Status);
       return user;
   }

   protected async Task<User> AuthorizeWithoutSubscription()
   {
       var user = UserFactory.Get();
       await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.AsDocument());
       Authorize(user.Id, user.Status);
       return user;
   }

   protected virtual void Authorize(Guid userId, string status)
   {
       var optionsProvider = new OptionsProvider();
       var authOptions = optionsProvider.Get<AuthOptions>("Auth");
       var authenticator = new JwtAuthenticator(new Clock(), Options.Create<AuthOptions>(authOptions));
       var token = authenticator.CreateToken(userId.ToString(), status);
       HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);
   }
   
   public void Dispose()
       => TestAppDb.Dispose();
}