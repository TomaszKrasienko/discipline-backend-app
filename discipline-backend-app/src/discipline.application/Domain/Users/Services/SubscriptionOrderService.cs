using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Services.Abstractions;

namespace discipline.application.Domain.Users.Services;

internal sealed class SubscriptionOrderService : ISubscriptionOrderService
{
    public void AddOrderSubscriptionToUser(User user, Guid id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now, string cardNumber, string cardCvvNumber)
    {
        if (subscription.IsFreeSubscription())
        {
            user.CreateFreeSubscriptionOrder(id, subscription, now);
        }
        else
        {
            
        }
    }
    
}
