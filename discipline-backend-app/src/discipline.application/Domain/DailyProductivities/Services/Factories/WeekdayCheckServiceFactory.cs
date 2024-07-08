using discipline.application.Domain.DailyProductivities.Services.Abstractions;
using discipline.application.Domain.DailyProductivities.Services.Internal;

namespace discipline.application.Domain.DailyProductivities.Services.Factories;

internal static class WeekdayCheckServiceFactory
{
    private static IWeekdayCheckService _weekdayCheckService;

    internal static IWeekdayCheckService GetInstance()
        => _weekdayCheckService ??= new WeekdayCheckService();
    
}