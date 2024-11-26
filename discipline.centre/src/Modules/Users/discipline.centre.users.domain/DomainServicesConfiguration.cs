using discipline.centre.users.domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.domain;

public static class DomainServicesConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddSingleton<ISubscriptionOrderService, SubscriptionOrderService>();
}