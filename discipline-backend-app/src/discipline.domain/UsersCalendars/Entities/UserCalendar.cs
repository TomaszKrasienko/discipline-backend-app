using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Aggregate;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Exceptions;
using discipline.domain.UsersCalendars.ValueObjects.UserCalendar;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class UserCalendar : AggregateRoot<UserCalendarId>
{
    private readonly List<Event> _events = [];
    public Day Day { get; }
    public UserId UserId { get; }
    public IReadOnlyList<Event> Events => _events;

    private UserCalendar(UserCalendarId id, Day day, UserId userId) : base(id)
    {
        Day = day;
        UserId = userId;
    } 
    
    //For mongo
    public UserCalendar(UserCalendarId id, Day day, UserId userId, List<Event> events) 
        : this(id, day, userId)
        => _events = events;

    public static UserCalendar Create(UserCalendarId id, DateOnly day, UserId userId)
        => new UserCalendar(id, day, userId);

    public void AddEvent(EventId id, string title)
        => _events.Add(ImportantDate.Create(id, title));

    public void AddEvent(EventId id, string title, TimeOnly timeFrom,
        TimeOnly? timeTo, string action)
        => _events.Add(CalendarEvent.Create(id, title, timeFrom, timeTo, action));

    public void AddEvent(EventId id, string title, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
        => _events.Add(Meeting.Create(id, title, timeFrom, timeTo, platform, uri, place));

    internal void AddEvent(Event @event)
        => _events.Add(@event);

    public void EditEvent(EventId id, string title)
    {
        var importantDate = GetEvent(id);
        ValidateEventType(importantDate, typeof(ImportantDate));
        ((ImportantDate)importantDate).Edit(title);
    }

    public void EditEvent(EventId id, string title, TimeOnly timeFrom,
        TimeOnly? timeTo, string action)
    {
        var calendarEvent = GetEvent(id);
        ValidateEventType(calendarEvent, typeof(CalendarEvent));
        ((CalendarEvent)calendarEvent).Edit(title, timeFrom, timeTo, action);
    }

    public void EditEvent(EventId id, string title, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
    {
        var meeting = GetEvent(id);
        ValidateEventType(meeting, typeof(Meeting));
        ((Meeting)meeting).Edit(title, timeFrom, timeTo, platform, uri, place);
    }

    private void ValidateEventType(Event @event, Type destinationType)
    {
        var type = @event.GetType();
        if (type != destinationType)
        {
            throw new InvalidEventTypeIdException(@event.Id);
        }
    }

    internal void RemoveEvent(EventId eventId)
    {
        var @event = GetEvent(eventId);
        if (@event is null)
        {
            throw new EventNotFoundException(eventId);
        }

        _events.Remove(@event);
    }
    

    private Event GetEvent(EventId id)
    {       
        var @event = _events.FirstOrDefault(x => x.Id == id);
        if (@event is null)
        {
            throw new EventNotFoundException(id);
        }
        return @event;
    }
}