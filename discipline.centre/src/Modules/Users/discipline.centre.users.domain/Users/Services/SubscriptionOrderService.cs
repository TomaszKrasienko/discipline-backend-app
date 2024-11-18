using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Users.Enums;

namespace discipline.centre.users.domain.Users.Services;

internal sealed class SubscriptionOrderService : ISubscriptionOrderService
{
    public static void AddOrderSubscriptionToUser(User user, SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTimeOffset now, string paymentToken)
    {
        if (user is null)
        {
            throw new DomainException("User.NotFound", "User not found");
        }

        if (subscription is null)
        {
            throw new DomainException("Subscription.NotFound", "Subscription not found");
        }
        
        if (subscription.IsFree())
        {
            user.CreateFreeSubscriptionOrder(id, subscription, now);
        }
        else
        {
            if (subscriptionOrderFrequency is null)
            {
                throw new DomainException("Subscription.Frequency.NotFound", "Subscription frequency not found");
            }
            user.CreatePaidSubscriptionOrder(id, subscription, subscriptionOrderFrequency.Value,
                now, paymentToken);
        }
    }
}