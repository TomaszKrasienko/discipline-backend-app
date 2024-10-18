using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public sealed class  FreeSubscriptionOrder : SubscriptionOrder
{
    private FreeSubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt) : base(id, createdAt)
    {
    }
    
    //for mongo
    public FreeSubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt,
        SubscriptionId subscriptionId, State state) : base(id, createdAt, subscriptionId, state)
    {
    }

    public static FreeSubscriptionOrder Create(SubscriptionOrderId id, Subscription subscription, DateTime now)
    {
        if (subscription is null)
        {
            throw new NullSubscriptionException();
        }

        if (!subscription.IsFreeSubscription())
        {
            throw new InvalidSubscriptionTypeException();
        }
        
        var freeSubscriptionOrder = new FreeSubscriptionOrder(id, now);
        freeSubscriptionOrder.ChangeSubscriptionId(subscription.Id);
        freeSubscriptionOrder.SetState(new State(false, null));
        return freeSubscriptionOrder;
    }
}