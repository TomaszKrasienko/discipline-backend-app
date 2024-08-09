using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.UsersCalendars.Exceptions;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

internal sealed record Title
{
    internal string Value { get; }

    internal Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyEventTitleException();
        }

        if (value.Length is < 3 or > 100)
        {
            throw new InvalidActivityRuleTitleLengthException(value);
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}