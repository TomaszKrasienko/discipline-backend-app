using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class SubscriptionOrderAlreadyPickedRule(UserId id,
    SubscriptionOrder? current, SubscriptionOrder next) : IBusinessRule
{
    public Exception Exception => new DomainException("User.SubscriptionOrder.AlreadyPicked",
        $"Subscription for User: {id} is already picked");
    
    public bool IsBroken()
    {
        if (current is null)
        {
            return false;
        }

        return (current is FreeSubscriptionOrder && next is FreeSubscriptionOrder)
            || (current is PaidSubscriptionOrder && next is PaidSubscriptionOrder);
    }
}