using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Users.Enums;

namespace discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class Type : ValueObject
{
    public SubscriptionOrderFrequency Value { get; }

    public static Type Create(SubscriptionOrderFrequency value)
        => new (value);
    
    private Type(SubscriptionOrderFrequency value)
        => Value = value;
    
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