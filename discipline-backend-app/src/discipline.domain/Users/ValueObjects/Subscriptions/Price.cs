using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules.Price;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects.Subscriptions;

public sealed class Price : ValueObject
{
    private readonly decimal _perMonth;
    private readonly decimal _perYear;

    public decimal PerMonth
    {
        get => _perMonth;
        private init
        {
            CheckRule(new PriceCanBeLessThanZeroRule(value, nameof(PerMonth)));
            _perMonth = value;
        }
    }

    public decimal PerYear
    {
        get => _perYear;
        private init
        {
            CheckRule(new PriceCanBeLessThanZeroRule(value, nameof(PerYear)));
            _perYear = value;
        }
    }

    public Price(decimal perMonth, decimal perYear)
    {
        PerMonth = perMonth;
        PerYear = perYear;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return PerMonth;
        yield return PerYear;
    }
}