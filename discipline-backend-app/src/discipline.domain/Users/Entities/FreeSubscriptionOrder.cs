using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public sealed class FreeSubscriptionOrder : SubscriptionOrder
{
    private FreeSubscriptionOrder(EntityId id, CreatedAt createdAt) : base(id, createdAt)
    {
    }
    
    //for mongo
    public FreeSubscriptionOrder(EntityId id, CreatedAt createdAt,
        EntityId subscriptionId, State state) : base(id, createdAt, subscriptionId, state)
    {
    }

    public static FreeSubscriptionOrder Create(Guid id, Subscription subscription, DateTime now)
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