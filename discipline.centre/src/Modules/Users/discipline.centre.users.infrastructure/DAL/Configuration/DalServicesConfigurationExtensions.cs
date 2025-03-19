using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Users.Repositories;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.infrastructure.DAL;
using discipline.centre.users.infrastructure.DAL.Subscriptions;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string assemblyName)
        => services
            .AddMongoContext<UsersMongoContext>()
            .AddScoped<IReadUserRepository, MongoUserRepository>()
            .AddScoped<IReadWriteUserRepository, MongoUserRepository>()
            .AddScoped<IReadSubscriptionRepository, MongoSubscriptionRepository>()
            .AddScoped<IReadWriteSubscriptionRepository, MongoSubscriptionRepository>()
            .AddHostedService<SubscriptionInitializer>();
}