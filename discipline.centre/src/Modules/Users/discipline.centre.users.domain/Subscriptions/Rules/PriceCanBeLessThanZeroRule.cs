using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Subscriptions.Rules;

internal sealed class PriceCanBeLessThanZeroRule(decimal value, string fieldName) : IBusinessRule
{
    public Exception Exception => new DomainException($"Subscription.Price.{fieldName}.LessThanZero",
        $"The price value: {value} is invalid. Value can not be less than zero");

    public bool IsBroken()
        => value < 0;
}