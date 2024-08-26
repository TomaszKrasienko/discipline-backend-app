using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal PerMonth { get; }
    public decimal PerYear { get; }

    public Price(decimal perMonth, decimal perYear)
    {
        if (perMonth < 0)
        {
            throw new SubscriptionValueLessThanZeroException(nameof(PerMonth));
        }

        if (perYear < 0)
        {
            throw new SubscriptionValueLessThanZeroException(nameof(PerYear));
        }
        PerMonth = perMonth;
        PerYear = perYear;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return PerMonth;
        yield return PerYear;
    }
}