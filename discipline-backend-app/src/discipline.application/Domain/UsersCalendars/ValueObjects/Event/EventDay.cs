namespace discipline.application.Domain.UsersCalendars.ValueObjects.Event;

internal sealed class EventDay
{
    internal DateOnly Value { get; }

    internal EventDay(DateOnly value)
    {
        Value = value;
    }
    
    public static implicit operator DateOnly(EventDay day)
        => day.Value;

    public static implicit operator EventDay(DateOnly dateTime)
        => new EventDay(dateTime);

    public static bool operator ==(EventDay day, DateOnly value)
        => day?.Value == value;

    public static bool operator !=(EventDay day, DateOnly value) 
        => !(day == value);
}