using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public abstract class Event
{
    public EntityId Id { get; }
    public Title Title { get; private set; }

    protected Event(EntityId id, Title title)
    {
        Id = id;
        ChangeTitle(title);
    }

    protected void ChangeTitle(string title)
        => Title = title;

}