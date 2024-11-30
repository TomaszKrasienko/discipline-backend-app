using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.dailytrackers.domain.Rules.Activities;

internal sealed class TitleCannotBeLongerThan20Rule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Activity.Details.Title.TooLong",
        $"Title: {value} is longer than 20.");
    public bool IsBroken()
        => value.Length > 20;
}