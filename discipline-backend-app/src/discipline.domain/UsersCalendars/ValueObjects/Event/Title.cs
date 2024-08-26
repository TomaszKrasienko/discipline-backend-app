using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.Exceptions;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed class Title : ValueObject
{
    public string Value { get; }

    public Title(string value)
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

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}