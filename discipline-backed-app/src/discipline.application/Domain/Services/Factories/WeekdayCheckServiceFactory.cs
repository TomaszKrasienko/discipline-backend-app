using discipline.application.Domain.Services.Abstractions;
using discipline.application.Domain.Services.Internal;

namespace discipline.application.Domain.Services.Factories;

internal static class WeekdayCheckServiceFactory
{
    private static IWeekdayCheckService _weekdayCheckService;

    internal static IWeekdayCheckService GetInstance()
        => _weekdayCheckService ??= new WeekdayCheckService();
    
}