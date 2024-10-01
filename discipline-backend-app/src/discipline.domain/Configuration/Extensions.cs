using discipline.domain.Users.Services;
using discipline.domain.Users.Services.Abstractions;
using discipline.domain.UsersCalendars.Services.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.domain.Configuration;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddUserCalendars()
            .AddSingleton<ISubscriptionOrderService, SubscriptionOrderService>();
}