using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class DetailsTitleCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Details.Title.Empty",
        "Activity title cannot be empty.");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}