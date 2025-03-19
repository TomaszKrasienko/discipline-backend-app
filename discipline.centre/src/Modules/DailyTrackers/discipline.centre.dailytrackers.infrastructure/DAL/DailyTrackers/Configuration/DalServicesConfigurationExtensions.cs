using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.CacheDecorators;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddMongoContext<DailyTrackersMongoContext>()
            .AddScoped<IReadWriteDailyTrackerRepository, MongoDailyTrackerRepository>()
            .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IReadWriteDailyTrackerRepository), typeof(CacheDailyTrackerRepositoryDecorator));

        return services;
    }
}