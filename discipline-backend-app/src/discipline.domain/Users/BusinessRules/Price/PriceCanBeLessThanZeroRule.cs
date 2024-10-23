using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Price;

internal sealed class PriceCanBeLessThanZeroRule(decimal value, string fieldName) : IBusinessRule
{
    public Exception Exception => new SubscriptionValueLessThanZeroException(fieldName);

    public bool IsBroken()
        => value < 0;
}