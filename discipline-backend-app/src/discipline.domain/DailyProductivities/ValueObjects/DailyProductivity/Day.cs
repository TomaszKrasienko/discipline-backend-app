using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.ValueObjects.DailyProductivity;

public sealed class Day : ValueObject
{
    public DateOnly Value { get; }

    public Day(DateOnly value)
    {
        Value = new DateOnly(value.Year, value.Month, value.Day);
    }

    public static implicit operator DateOnly(Day day)
        => day.Value;

    public static implicit operator Day(DateOnly dateTime)
        => new Day(dateTime);

    public static bool operator ==(Day day, DateOnly value)
        => day?.Value == value;

    public static bool operator !=(Day day, DateOnly value) 
        => !(day == value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}