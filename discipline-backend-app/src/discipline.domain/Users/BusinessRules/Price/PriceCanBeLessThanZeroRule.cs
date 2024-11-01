using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Price;

internal sealed class PriceCanBeLessThanZeroRule(decimal value, string fieldName) : IBusinessRule
{
    public Exception Exception => new DomainException($"Subscription.Price.{fieldName}.LessThanZero",
        $"The price value: {value} is invalid. Value can not be less than zero");

    public bool IsBroken()
        => value < 0;
}