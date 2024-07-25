using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class FreeSubscriptionOrder : SubscriptionOrder
{
    //for mongo
    internal FreeSubscriptionOrder(EntityId id, CreatedAt createdAt) : base(id, createdAt)
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
        //freeSubscriptionOrder.ChangeState(false, null);
        return freeSubscriptionOrder;
    }
}