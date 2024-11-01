using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.SubscriptionOrders;

internal sealed class SubscriptionMustBeValidTypeRule(Type type, Subscription subscription) : IBusinessRule
{
    public Exception Exception => new DomainException($"User.{type.Name}.InvalidType",
        $"Provided subscription: {subscription.Title} is invalid for: {type.Name}");

    public bool IsBroken()
        => (type == typeof(PaidSubscriptionOrder) && subscription.IsFree()) 
        || (type == typeof(FreeSubscriptionOrder) && !subscription.IsFree());
    
}