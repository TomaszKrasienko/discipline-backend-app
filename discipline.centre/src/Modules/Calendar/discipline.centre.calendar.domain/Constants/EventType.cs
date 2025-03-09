namespace discipline.centre.calendar.domain.Constants;

public record struct EventType(string Value)
{
    public EventType TimeEvent => new("TimeEvent");
    public EventType ImportantDate => new("ImportantDate");

    public EventType Parse(string value) => value switch
    {
        "TimeEvent" => TimeEvent,
        "ImportantDate" => ImportantDate,
        _ => throw new ArgumentException($"Cannot parse {value} into EventType")
    };
}