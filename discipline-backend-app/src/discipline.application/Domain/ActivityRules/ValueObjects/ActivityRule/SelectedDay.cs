using discipline.application.Domain.ActivityRules.Exceptions;

namespace discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;

internal sealed record SelectedDay
{
    public int Value { get; }

    internal SelectedDay(int value)
    {
        if (value is < (int)DayOfWeek.Sunday or > (int)DayOfWeek.Saturday)
        {
            throw new SelectedDayOutOfRangeException(value);
        }
        Value = value;
    }

    public static implicit operator int(SelectedDay day)
        => day.Value;

    public static implicit operator SelectedDay(int value)
        => new SelectedDay(value);
}