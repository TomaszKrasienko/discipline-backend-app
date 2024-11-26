using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Features;

internal sealed class FeatureCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Feature.Empty",
        "Feature value can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}