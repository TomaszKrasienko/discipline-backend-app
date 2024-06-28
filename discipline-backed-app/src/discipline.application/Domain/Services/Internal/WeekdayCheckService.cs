using discipline.application.Domain.Services.Abstractions;
using discipline.application.Domain.ValueObjects.ActivityRules;

namespace discipline.application.Domain.Services.Internal;

internal sealed class WeekdayCheckService : IWeekdayCheckService
{
    public bool IsDateForMode(DateTime now, string mode, List<int> selectedDays = null)
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