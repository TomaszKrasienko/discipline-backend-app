using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.dailytrackers.domain.Rules.Stages;

internal sealed class TitleCannotBeLongerThan30(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Stage.Title.TooLong",
        $"Title: {value} has invalid length");

    public bool IsBroken()
        => value.Length > 30;
}