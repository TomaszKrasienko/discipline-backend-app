using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.ValueObjects;

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
        => Price?.PerMonth == 0 && Price?.PerYear == 0;
}