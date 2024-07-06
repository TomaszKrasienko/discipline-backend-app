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

// internal sealed class Meeting : Event
// {
//     public TimeOnly TimeFrom { get; set; }
//     public TimeOnly TimeTo { get; set; }
//     public string Platform { get; set; }
//     public string MeetingAddress { get; set; }
//     public string Address { get; set; }
// }
//
// internal sealed class CalendarEvent : Event
// {
//     public TimeOnly TimeFrom { get; set; }
//     public TimeOnly TimeTo { get; set; }
//     public string Action { get; set; }
// }
