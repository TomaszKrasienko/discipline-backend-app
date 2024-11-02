using discipline.centre.shared.abstractions.SharedKernel;
using discipline.users.domain.Subscriptions.Rules;

namespace discipline.users.domain.Subscriptions.ValueObjects;

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

    public static Price Create(decimal perMonth, decimal perYear)
        => new Price(perMonth, perYear);

    private Price(decimal perMonth, decimal perYear)
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