using discipline.domain.DailyProductivities.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.ValueObjects.Activity;

public sealed class Title : ValueObject
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

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}