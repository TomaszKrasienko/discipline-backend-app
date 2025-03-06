using discipline.centre.calendar.domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discpline.centre.calendar.domain.ValueObjects;

namespace discpline.centre.calendar.domain;

public sealed class TimeEvent : BaseCalendarEvent
{
    public EventTimeSpan TimeSpan { get; }

    private TimeEvent(CalendarEventId id, EventTimeSpan timeSpan, DateOnly day, CalendarEventContent content) :  base(id, day, content) 
    {
        TimeSpan = timeSpan;
    }
}