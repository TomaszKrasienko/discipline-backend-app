using discipline.application.Domain.DailyProductivities.Services.Abstractions;
using discipline.application.Domain.DailyProductivities.Services.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Domain.Configuration;

internal static  class Extensions
{
    internal static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddSingleton<IWeekdayCheckService, WeekdayCheckService>();
}