using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class ModeCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Mode.Empty", 
        "Activity rule mode can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}