using discipline.domain.ActivityRules.Exceptions;

namespace discipline.domain.ActivityRules.ValueObjects.ActivityRule;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
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