using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.Services.Abstractions;

namespace discipline.application.Domain.Users.Services;

internal sealed class SubscriptionOrderService : ISubscriptionOrderService
{
    public void AddOrderSubscriptionToUser(User user, Guid id, Subscription subscription,
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
