using System.Net.Http.Headers;
using discipline.application.Behaviours;
using discipline.application.DTOs;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents.Mappers;
using discipline.infrastructure.DAL.Documents.Users;
using discipline.infrastructure.Time;
using discipline.tests.shared.Entities;
using Microsoft.AspNetCore.Http;
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
   
   protected virtual string GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
   {
       if (httpResponseMessage is null)
       {
           throw new InvalidOperationException("Http response message is null");
       }

       if (!httpResponseMessage.Headers.TryGetValues(ResourceHeaderExtension.HeaderName, out var value))
       {
           return null;
       }

       return value.Single();
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
       user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), subscription, DateTime.Now);
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

   protected virtual void Authorize(UserId userId, string status)
   {
       var optionsProvider = new OptionsProvider();
       var authOptions = optionsProvider.Get<AuthOptions>("Auth");
       var authenticator = new JwtAuthenticator(new Clock(), Options.Create<AuthOptions>(authOptions));
       var token = authenticator.CreateToken(userId.ToString(), status);
       HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
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