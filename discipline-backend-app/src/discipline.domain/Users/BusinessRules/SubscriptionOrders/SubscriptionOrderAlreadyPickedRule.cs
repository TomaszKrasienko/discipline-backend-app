using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.SubscriptionOrders;

internal sealed class SubscriptionOrderAlreadyPickedRule(UserId id,
    SubscriptionOrder? current, SubscriptionOrder next) : IBusinessRule
{
    public Exception Exception => new SubscriptionOrderForUserAlreadyExistsException(id);
    
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