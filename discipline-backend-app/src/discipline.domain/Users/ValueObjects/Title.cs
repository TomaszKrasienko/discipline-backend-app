using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptySubscriptionTitleException();
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}