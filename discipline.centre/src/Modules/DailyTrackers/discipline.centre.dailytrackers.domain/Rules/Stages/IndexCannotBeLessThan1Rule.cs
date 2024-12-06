using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.dailytrackers.domain.Rules.Stages;

internal sealed class IndexCannotBeLessThan1Rule(int value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Stage.Index.LessThanOne",
        "Stage index cannot be less than 1.");

    public bool IsBroken()
        => value < 1;
}