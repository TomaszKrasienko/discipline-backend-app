using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;

namespace discipline.domain.Users.Services.Abstractions;

public interface ISubscriptionOrderService
{
     void AddOrderSubscriptionToUser(User user, SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTimeOffset now,
        string cardNumber, string cardCvvNumber);
}