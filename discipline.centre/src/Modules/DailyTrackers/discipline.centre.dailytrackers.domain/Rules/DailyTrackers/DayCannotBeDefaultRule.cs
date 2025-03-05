using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.dailytrackers.domain.Rules.DailyTrackers;

internal sealed class DayCannotBeDefaultRule(DateOnly value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Day.Default",
        "Daily tracker day cannot be default value");

    public bool IsBroken()
        => value == DateOnly.MinValue;
}