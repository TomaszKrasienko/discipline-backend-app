using discipline.domain.Users.Services;
using discipline.domain.Users.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.domain.Configuration;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddSingleton<ISubscriptionOrderService, SubscriptionOrderService>();
}