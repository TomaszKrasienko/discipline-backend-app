using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class ModeCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Mode.Empty", 
        "Activity rule mode cannot be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}