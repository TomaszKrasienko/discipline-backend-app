using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using discipline.centre.users.domain.Users.Rules.SubscriptionOrders;
using discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders;

namespace discipline.centre.users.domain.Users;

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