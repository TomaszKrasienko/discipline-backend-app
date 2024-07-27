using discipline.application.Domain.Users.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Domain.Users.Services.Configuration;

internal static class Extensions
{
    internal static IServiceCollection SetDomain(this IServiceCollection services)
        => services.AddSingleton<ISubscriptionOrderService, SubscriptionOrderService>();
}