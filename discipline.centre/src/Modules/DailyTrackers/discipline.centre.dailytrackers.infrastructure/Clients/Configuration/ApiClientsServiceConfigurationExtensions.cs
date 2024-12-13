using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.infrastructure.Clients.ActivityRules;

// ReSharper disable once CheckNamespace
namespace  Microsoft.Extensions.DependencyInjection;

internal static class ApiClientsServiceConfigurationExtensions
{
    internal static IServiceCollection AddApiClients(this IServiceCollection services)
        => services
            .AddSingleton<IActivityRulesApiClient, ActivityRulesApiClient>();
}