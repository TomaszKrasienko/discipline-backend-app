using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.Subscriptions;

namespace discipline.domain.Users.BusinessRules.Features;

internal sealed class FeaturesCanNotBeEmptyRule(List<Feature> features) : IBusinessRule
{
    public Exception Exception => new DomainException("Subscription.Features.EmptyList",
        "Features list can not be empty");

    public bool IsBroken()
        => features.Count == 0;
}