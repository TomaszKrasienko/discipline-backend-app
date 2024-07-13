using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;
using Action = discipline.application.Domain.UsersCalendars.ValueObjects.Event.Action;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal sealed class CalendarEvent : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; set; }
    public Action Action { get; set; }

    private CalendarEvent(EntityId id, Title title) : base(id, title)
    {
    }

    //For mongo
    public CalendarEvent(EntityId id, Title title, MeetingTimeSpan meetingTimeSpan, Action action) : base(id, title)
    {
        MeetingTimeSpan = meetingTimeSpan;
        Action = action;
    }

    internal static CalendarEvent Create(Guid id, string title, TimeOnly timeFrom, 
        TimeOnly? timeTo, string action)
    {
        var calendarEvent = new CalendarEvent(id, title);
        calendarEvent.ChangeMeetingTimeSpan(timeFrom, timeTo);
        calendarEvent.ChangeAction(action);
        return calendarEvent;
    }
    
    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeAction(string action)
        => Action = action;
}