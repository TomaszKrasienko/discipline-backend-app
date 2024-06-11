using discipline.application.Domain.Exceptions;

namespace discipline.application.Domain.ValueObjects.ActivityRule;

internal sealed record Title
{
    private string Value { get; }

    internal Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityRuleTitleException();
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}