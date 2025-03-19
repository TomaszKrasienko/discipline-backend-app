using discipline.centre.calendar.domain.Repositories;
using discipline.centre.calendar.infrastructure.DAL;
using discipline.centre.calendar.infrastructure.DAL.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services
            .AddMongoContext<CalendarMongoContext>()
            .AddScoped<IReadUserCalendarRepository, MongoUserCalendarDayRepository>()
            .AddScoped<IReadWriteUserCalendarRepository, MongoUserCalendarDayRepository>();
}