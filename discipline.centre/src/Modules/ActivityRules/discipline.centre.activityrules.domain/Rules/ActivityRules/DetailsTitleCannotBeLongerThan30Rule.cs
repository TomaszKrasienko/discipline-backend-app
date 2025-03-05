using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.ActivityRules;

internal sealed class DetailsTitleCannotBeLongerThan30Rule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Details.Title.TooLong",
        $"Title: {value} is longer than 30.");
    public bool IsBroken()
        => value.Length > 30;
}