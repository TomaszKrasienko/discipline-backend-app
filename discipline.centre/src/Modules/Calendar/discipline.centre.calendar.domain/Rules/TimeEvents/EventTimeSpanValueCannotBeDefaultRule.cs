using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.calendar.domain.Rules.TimeEvents;

internal sealed class EventTimeSpanValueCannotBeDefaultRule(TimeSpan value) : IBusinessRule
{
    public Exception Exception => new DomainException("TimeEvent.EventTimeSpan.Defualt",
        "Time event time span value cannot be default.");
    public bool IsBroken()
        => value == TimeSpan.Zero;
}