using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record Cost
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
}