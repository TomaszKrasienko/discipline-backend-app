using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Title.Empty",
        "Activity rule title can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}