using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Subscriptions.Rules;

internal sealed class FeatureCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Feature.Empty",
        "Feature value can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}