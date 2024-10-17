using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public abstract class Event : Entity<Ulid>
{
    public Ulid Id { get; }
    public Title Title { get; private set; }

    protected Event(Ulid id) : base(id)
    {
        
    }
    
    protected Event(Ulid id, string title) : this(id)
        => Title = title;

    protected void ChangeTitle(string title)
        => Title = title;

}