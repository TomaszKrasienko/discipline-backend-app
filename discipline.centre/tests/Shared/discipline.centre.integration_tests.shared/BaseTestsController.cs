using System.Net.Http.Headers;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Clock;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using discipline.centre.users.domain.Users;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.infrastructure.Users.Auth;
using discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;
using discipline.centre.users.tests.sharedkernel.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.centre.integration_tests.shared;

public abstract class BaseTestsController : IDisposable
{
    public TestAppDb TestAppDb { get; set; }
    protected HttpClient HttpClient { get; set; }

    protected BaseTestsController(string moduleName)
    {
        TestAppDb = new TestAppDb(moduleName);
        var app = new TestApp(ConfigureServices);
        HttpClient = app.HttpClient;
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoCollectionNameConvention, TestsMongoCollectionNameConvention>();
        services.AddSingleton<IMongoClient>(sp => TestAppDb.GetMongoClient());
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
    
    protected async Task<User> AuthorizeWithFreeSubscriptionPicked()
    {
        var subscription = SubscriptionFakeDataFactory.Get();
        var user = UserFakeDataFactory.Get();
        user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), subscription, DateTime.Now);
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.MapAsDocument(Guid.NewGuid().ToString()));
        Authorize(user.Id, user.Email, user.Status);
        return user;
    }

    protected async Task<User> AuthorizeWithoutSubscription()
    {
        var user = UserFakeDataFactory.Get();
        await TestAppDb.GetCollection<UserDocument>().InsertOneAsync(user.MapAsDocument(Guid.NewGuid().ToString()));
        Authorize(user.Id, user.Email, user.Status);
        return user;
    }
    
    protected virtual void Authorize(UserId userId, string email, string status)
    {
        var optionsProvider = new OptionsProvider();
        var authOptions = optionsProvider.Get<JwtOptions>();
        var authenticator = new JwtAuthenticator(new Clock(), Options.Create(authOptions));
        var token = authenticator.CreateToken(userId.ToString(), email, status);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }
    
    protected virtual string? GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
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
}