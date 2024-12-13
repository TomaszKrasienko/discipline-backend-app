using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string assemblyName)
        => services
            .AddMongoContext<DailyTrackersMongoContext>()
            .AddScoped<IReadDailyTrackerRepository, MongoDailyTrackerRepository>()
            .AddScoped<IWriteReadDailyTrackerRepository, MongoDailyTrackerRepository>();
}