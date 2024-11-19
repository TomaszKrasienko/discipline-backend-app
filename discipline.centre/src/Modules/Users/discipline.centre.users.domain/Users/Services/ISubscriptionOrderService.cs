using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Users.Enums;

namespace discipline.centre.users.domain.Users.Services;

public interface ISubscriptionOrderService
{
    void AddOrderSubscriptionToUser(User user, SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTimeOffset now,
        string? paymentToken);
}