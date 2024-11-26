using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.DailyProductivities.Services.Abstractions;

namespace discipline.domain.DailyProductivities.Services.Internal;

internal sealed class WeekdayCheckService : IWeekdayCheckService
{
    public static IWeekdayCheckService GetInstance()
        => new WeekdayCheckService();

    public bool IsDateForMode(DateTimeOffset now, string mode, List<int> selectedDays = null)
    {
        if (mode == Mode.EveryDayMode())
            return true;
        if (mode == Mode.FirstDayOfWeekMode() && now.DayOfWeek == DayOfWeek.Monday)
            return true;
        if (mode == Mode.LastDayOfWeekMode() && now.DayOfWeek == DayOfWeek.Sunday)
            return true;
        if (mode == Mode.FirstDayOfMonth() && now.Day == 1)
            return true;
        if (mode == Mode.LastDayOfMonthMode() && now.Day == DateTime.DaysInMonth(now.Year, now.Month))
            return true;
        if (mode == Mode.CustomMode() && (selectedDays?.Contains((int)now.DayOfWeek) ?? false))
            return true;
        return false;
    }
}