using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;

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