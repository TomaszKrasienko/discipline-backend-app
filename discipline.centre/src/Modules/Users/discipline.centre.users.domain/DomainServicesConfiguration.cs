using discipline.centre.users.domain.Users.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DomainServicesConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddSingleton<ISubscriptionOrderService, SubscriptionOrderService>();
}