using discipline.domain.SharedKernel;

namespace discipline.domain.UsersCalendars.ValueObjects.Event;

public sealed class Action : ValueObject
{
    public string Value { get; }

    private Action(string value)
    {
        Value = value;
    }

    public static implicit operator string(Action action)
        => action?.Value;

    public static implicit operator Action(string value)
        => new Action(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}