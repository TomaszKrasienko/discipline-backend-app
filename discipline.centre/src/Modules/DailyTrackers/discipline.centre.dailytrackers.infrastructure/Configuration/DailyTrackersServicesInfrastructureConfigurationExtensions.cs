// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions method for setting services configuration 
/// </summary>
public static class DailyTrackersServicesInfrastructureConfigurationExtensions
{
    /// <summary>
    /// Adds infrastructure services to DI container
    /// </summary>
    /// <param name="services">DI container, for more see <see cref="IServiceCollection"/></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddDal()
            .AddApiClients();
}