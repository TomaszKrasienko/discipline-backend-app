using discipline.centre.calendar.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.calendar.domain;

public sealed class TimeEvent : BaseCalendarEvent
{
    public EventTimeSpan TimeSpan { get; }

    private TimeEvent(CalendarEventId id, 
        EventTimeSpan timeSpan, 
        CalendarEventContent content) :  base(id, content) 
    {
        TimeSpan = timeSpan;
    }

    public static TimeEvent Create(CalendarEventId id,
        TimeOnly timeFrom, 
        TimeOnly? timeTo, 
        string title, 
        string? description)
    {
        var eventTimeSpan = EventTimeSpan.Create(timeFrom, timeTo);
        var eventContent = CalendarEventContent.Create(title, description);

        return new TimeEvent(id, eventTimeSpan, eventContent);
    }
}