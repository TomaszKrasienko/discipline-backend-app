using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.users.domain.Subscriptions.ValueObjects;

namespace discipline.users.domain.Subscriptions.Rules;

internal sealed class FeaturesCanNotBeEmptyRule(List<Feature> features) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Features.EmptyList",
        "Features list can not be empty");

    public bool IsBroken()
        => features.Count == 0;
}