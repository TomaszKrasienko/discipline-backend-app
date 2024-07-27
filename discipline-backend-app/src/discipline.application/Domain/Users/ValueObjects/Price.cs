using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record Price
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
}