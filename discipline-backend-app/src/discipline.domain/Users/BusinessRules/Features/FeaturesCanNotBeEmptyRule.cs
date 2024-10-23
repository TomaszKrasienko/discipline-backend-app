using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.BusinessRules.Features;

internal sealed class FeaturesCanNotBeEmptyRule(List<Feature> features) : IBusinessRule
{
    public Exception Exception => new EmptyFeaturesListException();

    public bool IsBroken()
        => features.Count == 0;
}