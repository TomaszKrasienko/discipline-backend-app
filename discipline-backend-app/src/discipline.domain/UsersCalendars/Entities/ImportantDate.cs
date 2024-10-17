using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class ImportantDate : Event
{
    //For mongo
    public ImportantDate(Ulid id, string title) : base(id, title)
    {
    }    
    
    private ImportantDate(Ulid id) : base(id)
    {
    }

    internal static ImportantDate Create(Ulid id, string title)
    {
       var @event = new ImportantDate(id);
       @event.ChangeTitle(title);
       return @event;
    } 

    internal void Edit(string title)
    {
        ChangeTitle(title);
    }
}