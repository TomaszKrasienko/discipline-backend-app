using discipline.application.Domain.Services.Abstractions;
using discipline.application.Domain.Services.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Domain.Configuration;

internal static  class Extensions
{
    internal static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddSingleton<IWeekdayCheckService, WeekdayCheckService>();
}