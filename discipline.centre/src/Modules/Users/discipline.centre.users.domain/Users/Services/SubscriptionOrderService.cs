using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Users.Enums;

namespace discipline.centre.users.domain.Users.Services;

internal sealed class SubscriptionOrderService : ISubscriptionOrderService
{
    public void AddOrderSubscriptionToUser(User user, SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTimeOffset now, string? paymentToken)
    {
        if (subscription.IsFree())
        {
            user.CreateFreeSubscriptionOrder(id, subscription, now);
        }
        else
        {
            if (subscriptionOrderFrequency is null)
            {
                throw new DomainException("SubscriptionOrder.Frequency.NotFound", "Subscription frequency not found");
            }

            if (paymentToken is null)
            {
                throw new DomainException("SubscriptionOrder.PaymentToken.Null", "Payment token can not be null");
            }
            user.CreatePaidSubscriptionOrder(id, subscription, subscriptionOrderFrequency.Value,
                now, paymentToken);
        }
    }
}