using discipline.domain.ActivityRules.Exceptions;

namespace discipline.domain.ActivityRules.ValueObjects.ActivityRule;

public sealed record SelectedDay
{
    public int Value { get; }

    public SelectedDay(int value)
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