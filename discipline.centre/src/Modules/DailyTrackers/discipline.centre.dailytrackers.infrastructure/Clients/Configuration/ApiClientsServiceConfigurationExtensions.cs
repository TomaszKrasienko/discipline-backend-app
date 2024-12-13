using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.infrastructure.Clients.ActivityRules;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.dailytrackers.infrastructure.Clients.Configuration;

internal static class ApiClientsServiceConfigurationExtensions
{
    internal static IServiceCollection AddApiClients(this IServiceCollection services)
        => services
            .AddSingleton<IActivityRulesApiClient, ActivityRulesApiClient>();
}