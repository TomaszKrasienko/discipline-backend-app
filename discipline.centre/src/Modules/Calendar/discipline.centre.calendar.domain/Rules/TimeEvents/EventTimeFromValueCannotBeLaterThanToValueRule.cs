using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.calendar.domain.Rules.TimeEvents;

internal sealed class EventTimeFromValueCannotBeLaterThanToValueRule(TimeOnly from, 
    TimeOnly? to) : IBusinessRule
{
    public Exception Exception => new DomainException("TimeEvent.EventTimeSpan.TooHighToValue",
        "Event time from  value cannot be later than to value.");
    public bool IsBroken()
        => from > to;
}