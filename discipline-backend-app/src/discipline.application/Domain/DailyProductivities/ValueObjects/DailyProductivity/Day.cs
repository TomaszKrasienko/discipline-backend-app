namespace discipline.application.Domain.DailyProductivities.ValueObjects.DailyProductivity;

internal sealed record Day
{
    internal DateOnly Value { get; }

    internal Day(DateOnly value)
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
}