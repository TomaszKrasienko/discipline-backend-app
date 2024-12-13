// ReSharper disable once CheckNamespace

using discipline.centre.dailytrackers.infrastructure.Clients.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DailyTrackersServicesInfrastructureConfigurationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string assemblyName)
        => services
            .AddDal(assemblyName)
            .AddApiClients();
}