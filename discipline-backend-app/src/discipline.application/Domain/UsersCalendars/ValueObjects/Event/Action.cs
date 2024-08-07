namespace discipline.application.Domain.UsersCalendars.ValueObjects.Event;

internal sealed record Action(string Value)
{
    public static implicit operator string(Action action)
        => action?.Value;

    public static implicit operator Action(string value)
        => new Action(value);
}