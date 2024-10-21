using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public abstract class Event : Entity<EventId>
{
    public Title Title { get; private set; }

    protected Event(EventId id) : base(id)
    {
        
    }
    
    protected Event(EventId id, string title) : this(id)
        => Title = title;

    protected void ChangeTitle(string title)
        => Title = title;

}