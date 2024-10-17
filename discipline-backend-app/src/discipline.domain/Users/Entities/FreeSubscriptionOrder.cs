using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public sealed class FreeSubscriptionOrder : SubscriptionOrder
{
    private FreeSubscriptionOrder(Ulid id, CreatedAt createdAt) : base(id, createdAt)
    {
    }
    
    //for mongo
    public FreeSubscriptionOrder(Ulid id, CreatedAt createdAt,
        Guid subscriptionId, State state) : base(id, createdAt, subscriptionId, state)
    {
    }

    public static FreeSubscriptionOrder Create(Ulid id, Subscription subscription, DateTime now)
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