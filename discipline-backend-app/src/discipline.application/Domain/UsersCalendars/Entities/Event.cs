using discipline.application.Domain.UsersCalendars.ValueObjects.Events;
using discipline.application.Domain.ValueObjects.SharedKernel;
using Action = discipline.application.Domain.UsersCalendars.ValueObjects.Events.Action;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal abstract class Event
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public EventDay EventDay { get; private set; }

    protected Event(EntityId id, Title title, EventDay eventDay)
    {
        Id = id;
        ChangeTitle(title);
        ChangeEventDay(eventDay);
    }

    protected void ChangeTitle(string title)
        => Title = title;

    protected void ChangeEventDay(DateOnly eventDay)
        => EventDay = eventDay;
}

internal sealed class ImportantDate : Event
{
    private ImportantDate(EntityId id, Title title, EventDay eventDay) : base(id, title, eventDay)
    {
    }

    internal static ImportantDate Create(Guid id, string title, DateOnly eventDay)
        => new ImportantDate(id, title, eventDay);
        
}

internal sealed class Meeting : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; private set; }
    public Address Address { get; private set; }


    private Meeting(EntityId id, Title title, EventDay eventDay) : base(id, title, eventDay)
    {
    }

    internal static Meeting Create(Guid id, string title, DateOnly eventDay, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
    {
        var @event = new Meeting(id, title, eventDay);
        @event.ChangeMeetingTimeSpan(timeFrom, timeTo);
        @event.ChangeMeetingAddress(platform, uri, place);
        return @event;
    }

    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeMeetingAddress(string platform, string uri, string place)
        => Address = new Address(platform, uri, place);
}

internal sealed class CalendarEvent : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; set; }
    public Action Action { get; set; }

    private CalendarEvent(EntityId id, Title title, EventDay eventDay) : base(id, title, eventDay)
    {
    }

    internal static CalendarEvent Create(Guid id, string title, DateOnly eventDay, 
        TimeOnly timeFrom, TimeOnly? timeTo, string action)
    {
        var calendarEvent = new CalendarEvent(id, title, eventDay);
        calendarEvent.ChangeMeetingTimeSpan(timeFrom, timeTo);
        calendarEvent.ChangeAction(action);
        return calendarEvent;
    }
    
    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeAction(string action)
        => Action = action;
}
