using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.users.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class Next(DateOnly value) : ValueObject
{
    public DateOnly Value { get; } = value;
    
    public static implicit operator DateOnly(Next next)
        => next.Value;
    
    public static implicit operator Next(DateOnly value)
        => new (value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}