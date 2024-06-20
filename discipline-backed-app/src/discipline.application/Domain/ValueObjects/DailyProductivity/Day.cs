namespace discipline.application.Domain.ValueObjects.DailyProductivity;

internal sealed record Day
{
    internal DateTime Value { get; }

    internal Day(DateTime value)
    {
        Value = new DateTime(value.Year, value.Month, value.Day);
    }

    public static implicit operator DateTime(Day day)
        => day.Value;

    public static implicit operator Day(DateTime dateTime)
        => new Day(dateTime);
}