using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Features;

internal sealed class FeatureCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new EmptyFeatureValueException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}