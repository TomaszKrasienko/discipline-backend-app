using discipline.application.Domain.ValueObjects.ActivityRules;

namespace discipline.application.Behaviours;

public class WeekdaysCheckingBehaviour
{
    
}

interface IWeekdayCheck
{
    bool IsDateForMode(DateTime now, string mode);
}

internal sealed class WeekdayCheck() 
    : IWeekdayCheck
{
    public bool IsDateForMode(DateTime now, string mode)
    {
        if (mode == Mode.EveryDayMode())
            return true;
        if (mode == Mode.FirstDayOfWeekMode() && now.DayOfWeek == DayOfWeek.Monday)
            return true;

    }
} 