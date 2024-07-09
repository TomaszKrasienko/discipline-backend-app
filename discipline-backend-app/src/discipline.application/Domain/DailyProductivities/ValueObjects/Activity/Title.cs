using discipline.application.Domain.DailyProductivities.Exceptions;

namespace discipline.application.Domain.DailyProductivities.ValueObjects.Activity;

internal sealed record Title
{
    internal string Value { get; }

    internal Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityTitleException();
        }
        
        if (value.Length is < 3 or > 100)
        {
            throw new InvalidActivityTitleLengthException(value);
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}