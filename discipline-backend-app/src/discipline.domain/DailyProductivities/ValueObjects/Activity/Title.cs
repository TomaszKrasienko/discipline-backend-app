using discipline.domain.DailyProductivities.Exceptions;

namespace discipline.domain.DailyProductivities.ValueObjects.Activity;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
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