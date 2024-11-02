using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.users.domain.Subscriptions;

namespace discipline.centre.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class SubscriptionMustBeValidTypeRule(Type type, Subscription subscription) : IBusinessRule
{
    public Exception Exception => new DomainException($"User.{type.Name}.InvalidType",
        $"Provided subscription: {subscription.Title} is invalid for: {type.Name}");

    public bool IsBroken()
        => (type == typeof(PaidSubscriptionOrder) && subscription.IsFree()) 
        || (type == typeof(FreeSubscriptionOrder) && !subscription.IsFree());
    
}