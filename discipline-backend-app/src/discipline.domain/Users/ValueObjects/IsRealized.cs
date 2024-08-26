using discipline.domain.SharedKernel;

namespace discipline.domain.Users.ValueObjects;

public sealed class IsRealized(bool value) : ValueObject
{
    public bool Value { get; } = value;

    public static implicit operator bool(IsRealized isRealized)
        => isRealized.Value;

    public static implicit operator IsRealized(bool value)
        => new IsRealized(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}