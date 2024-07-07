using discipline.application.Domain.ValueObjects.Events;
using discipline.application.Domain.ValueObjects.SharedKernel;
using Quartz;

namespace discipline.application.Domain.Entities;

internal abstract class Event
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public EventDay EventDay { get; private set; }

    protected Event(EntityId id)
    {
        Id = id;
    }

    protected void ChangeTitle(string title)
        => Title = title;

    protected void ChangeEventDay(DateOnly eventDay)
        => EventDay = eventDay;
}

internal sealed class ImportantDate : Event
{
    private ImportantDate(EntityId id) : base(id)
    {
    }

    internal static ImportantDate Create(Guid id, string title, DateOnly eventDay)
    {
        var @event = new ImportantDate(id);
        @event.ChangeTitle(title);
        @event.ChangeEventDay(eventDay);
        return @event;
    }
}

internal sealed class Meeting : Event
{
    public MeetingTimeSpan MeetingTimeSpan { get; private set; }
    public Address Address { get; private set; }


    private Meeting(EntityId id) : base(id)
    {
    }

    internal static Meeting Create(Guid id, string title, DateOnly eventDay, TimeOnly timeFrom, TimeOnly? timeTo,
        string platform, string uri, string place)
    {
        var @event = new Meeting(id);
        @event.ChangeTitle(title);
        @event.ChangeEventDay(eventDay);
        @event.ChangeMeetingTimeSpan(timeFrom, timeTo);
        @event.ChangeMeetingAddress(platform, uri, place);
        return @event;
    }

    private void ChangeMeetingTimeSpan(TimeOnly timeFrom, TimeOnly? timeTo)
        => MeetingTimeSpan = new MeetingTimeSpan(timeFrom, timeTo);

    private void ChangeMeetingAddress(string platform, string uri, string place)
        => Address = new Address(platform, uri, place);
}

// internal sealed class CalendarEvent : Event
// {
//     public TimeOnly TimeFrom { get; set; }
//     public TimeOnly TimeTo { get; set; }
//     public string Action { get; set; }
// }
