using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.dailytrackers.domain.Rules.Activities;

internal sealed class DetailsTitleCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Activity.Details.Title.Empty",
        "Activity title cannot be empty.");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}