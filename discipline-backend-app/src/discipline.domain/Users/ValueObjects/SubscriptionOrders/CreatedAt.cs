using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.CreatedAt;

namespace discipline.domain.Users.ValueObjects.SubscriptionOrders;

public sealed class CreatedAt : ValueObject
{
    private readonly DateTime _value;
    public DateTime Value
    {
        get => _value;
        private init
        {
            CheckRule(new CreatedAtCanNoBeDefaultRule(value));
            _value = value;
        }
    }

    private CreatedAt(DateTime value)
        => Value = value;

    public static implicit operator DateTime(CreatedAt createdAt)
        => createdAt.Value;

    public static implicit operator CreatedAt(DateTime value)
        => new(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}