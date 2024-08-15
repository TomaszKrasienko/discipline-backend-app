using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;

namespace discipline.domain.Users.Services.Abstractions;

public interface ISubscriptionOrderService
{
     void AddOrderSubscriptionToUser(User user, Guid id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber);
}