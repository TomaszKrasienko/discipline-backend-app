using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;

namespace discipline.application.Domain.Users.Services.Abstractions;

internal interface ISubscriptionOrderService
{
     Task AddOrderSubscriptionToUser(User user, Guid id, Subscription subscription,
        SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber);
}