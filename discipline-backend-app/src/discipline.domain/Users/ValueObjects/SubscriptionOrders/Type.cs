using discipline.domain.SharedKernel;
using discipline.domain.Users.Enums;

namespace discipline.domain.Users.ValueObjects;

public sealed class Type(SubscriptionOrderFrequency value) : ValueObject
{
    public SubscriptionOrderFrequency Value { get; } = value;
    
    public static implicit operator Type(SubscriptionOrderFrequency value)
        => new(value);

    public static implicit operator SubscriptionOrderFrequency? (Type type)
        => type?.Value;

    public static implicit operator SubscriptionOrderFrequency(Type type)
        => type.Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}