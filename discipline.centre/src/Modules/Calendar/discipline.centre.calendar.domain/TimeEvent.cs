using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class TimeEvent : BaseCalendarEvent
{
    public EventTimeSpan TimeSpan { get; }

    private TimeEvent(CalendarEventId id, EventTimeSpan timeSpan, DateOnly day, CalendarEventContent content) :  base(id, day, content) 
    {
        TimeSpan = timeSpan;
    }
}