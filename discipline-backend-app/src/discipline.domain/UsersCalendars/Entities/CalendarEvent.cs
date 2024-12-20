using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.ValueObjects.Event;
using Action = discipline.domain.UsersCalendars.ValueObjects.Event.Action;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class CalendarEvent : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; set; }
    public Action Action { get; set; }

    private CalendarEvent(EventId id) : base(id)
    {
    }

    //For mongo
    public CalendarEvent(EventId id, Title title, MeetingTimeSpan meetingTimeSpan, Action action)
        : base(id, title)
    {
        MeetingTimeSpan = meetingTimeSpan;
        Action = action;
    }

    internal static CalendarEvent Create(EventId id, string title, TimeOnly timeFrom, 
        TimeOnly? timeTo, string action)
    {
        var calendarEvent = new CalendarEvent(id);
        calendarEvent.ChangeTitle(title);
        calendarEvent.ChangeMeetingTimeSpan(timeFrom, timeTo);
        calendarEvent.ChangeAction(action);
        return calendarEvent;
    }

    internal void Edit(string title, TimeOnly timeFrom, TimeOnly? timeTo, string action)
    {
        ChangeTitle(title);
        ChangeMeetingTimeSpan(timeFrom, timeTo);
        ChangeAction(action);
    }
    
    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeAction(string action)
        => Action = action;
}