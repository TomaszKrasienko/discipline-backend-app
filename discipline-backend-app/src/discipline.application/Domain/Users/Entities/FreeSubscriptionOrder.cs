using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class FreeSubscriptionOrder : SubscriptionOrder
{
    private FreeSubscriptionOrder(EntityId id, CreatedAt createdAt) : base(id, createdAt)
    {
    }
    
    //for mongo
    internal FreeSubscriptionOrder(EntityId id, CreatedAt createdAt,
        EntityId subscriptionId, State state) : base(id, createdAt, subscriptionId, state)
    {
    }

    internal static FreeSubscriptionOrder Create(Guid id, Subscription subscription, DateTime now)
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