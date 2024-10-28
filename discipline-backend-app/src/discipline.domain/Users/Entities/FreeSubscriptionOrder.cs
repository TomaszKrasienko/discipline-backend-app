using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.BusinessRules.SubscriptionOrders;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;

namespace discipline.domain.Users.Entities;

public sealed class  FreeSubscriptionOrder : SubscriptionOrder
{
    public FreeSubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt,
        SubscriptionId subscriptionId, State state) : base(id, createdAt, subscriptionId, state)
    {
    }

    public static FreeSubscriptionOrder Create(SubscriptionOrderId id, Subscription subscription, DateTimeOffset now)
    {
        CheckRule(new SubscriptionMustBeValidTypeRule(typeof(FreeSubscriptionOrder), subscription));
        var state = new State(false, null);
        var freeSubscriptionOrder = new FreeSubscriptionOrder(id, now, subscription.Id,
            state);
        return freeSubscriptionOrder;
    }
}