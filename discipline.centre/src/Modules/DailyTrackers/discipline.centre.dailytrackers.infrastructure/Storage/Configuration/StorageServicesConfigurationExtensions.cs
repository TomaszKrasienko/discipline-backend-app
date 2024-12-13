using discipline.centre.dailytrackers.application.DailyTrackers.Services;
using discipline.centre.dailytrackers.infrastructure.Storage;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class StorageServicesConfigurationExtensions
{
    internal static IServiceCollection AddStorage(this IServiceCollection services)
        => services
            .AddScoped<IActivityIdStorage, HttpContextActivityIdStorage>();
}