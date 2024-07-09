using discipline.application.Domain.ActivityRules.Exceptions;

namespace discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;

internal sealed record Title
{
    internal string Value { get; }

    internal Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityRuleTitleException();
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