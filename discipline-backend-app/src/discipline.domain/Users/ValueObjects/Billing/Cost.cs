using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

//TODO: to change
public sealed class Cost : ValueObject
{
    public decimal Value { get; }
    
    internal Cost(decimal value)
    {
        if (value < 0)
        {
            throw new BillingValueLessThanZeroException(value);
        }

        Value = value;
    }

    public static implicit operator decimal(Cost cost)
        => cost.Value;

    public static implicit operator Cost(decimal value)
        => new Cost(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}