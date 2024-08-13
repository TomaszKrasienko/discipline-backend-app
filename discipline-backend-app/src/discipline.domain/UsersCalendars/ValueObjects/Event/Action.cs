namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed record Action(string Value)
{
    public static implicit operator string(Action action)
        => action?.Value;

    public static implicit operator Action(string value)
        => new Action(value);
}