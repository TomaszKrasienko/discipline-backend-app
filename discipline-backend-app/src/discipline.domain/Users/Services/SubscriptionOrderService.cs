using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.Services.Abstractions;

namespace discipline.domain.Users.Services;

public sealed class SubscriptionOrderService : ISubscriptionOrderService
{
    public void AddOrderSubscriptionToUser(User user, SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now, string cardNumber, string cardCvvNumber)
    {
        if (user is null)
        {
            throw new NullUserException();
        }

        if (subscription is null)
        {
            throw new NullSubscriptionException();
        }
        
        if (subscription.IsFreeSubscription())
        {
            user.CreateFreeSubscriptionOrder(id, subscription, now);
        }
        else
        {
            if (subscriptionOrderFrequency is null)
            {
                throw new NullSubscriptionOrderFrequencyException();
            }
            user.CreatePaidSubscriptionOrder(id, subscription, subscriptionOrderFrequency.Value,
                now, cardNumber, cardCvvNumber);
        }
    }
    
}
