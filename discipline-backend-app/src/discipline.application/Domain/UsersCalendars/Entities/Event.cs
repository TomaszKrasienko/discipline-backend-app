using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal abstract class Event
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