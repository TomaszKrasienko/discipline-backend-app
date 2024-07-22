using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.Entities;

internal sealed class Subscription
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Price Price { get; private set; }

    private Subscription(EntityId id)
        => Id = id;
    
    //For mongo
    internal Subscription(EntityId id, Title title, Price price) : this(id)
    {
        Title = title;
        Price = price;
    }

    internal static Subscription Create(Guid id, string title, decimal pricePerMonth, decimal pricePerYear)
    {
        var subscription = new Subscription(id);
        subscription.ChangeTitle(title);
        subscription.ChangePrice(pricePerMonth, pricePerYear);
        return subscription;
    }

    private void ChangeTitle(string title)
        => Title = title;

    private void ChangePrice(decimal pricePerMonth, decimal pricePerYear)
        => Price = new Price(pricePerMonth, pricePerYear);

    internal bool IsFreeSubscription()
        => true;
}

internal sealed record Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptySubscriptionTitleException();
        }
        Value = value;
    }

    public static implicit operator string(Title title)
        => title.Value;

    public static implicit operator Title(string value)
        => new Title(value);
}

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