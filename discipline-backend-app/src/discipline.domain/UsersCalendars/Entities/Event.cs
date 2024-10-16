using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public abstract class Event : Entity<Guid>
{
    public Guid Id { get; }
    public Title Title { get; private set; }

    protected Event(Guid id) : base(id)
    {
        
    }
    
    protected Event(Guid id, string title) : this(id)
        => Title = title;

    protected void ChangeTitle(string title)
        => Title = title;

}