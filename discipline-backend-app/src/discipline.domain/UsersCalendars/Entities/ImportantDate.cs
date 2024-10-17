using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class ImportantDate : Event
{
    //For mongo
    public ImportantDate(EventId id, string title) : base(id, title)
    {
    }    
    
    private ImportantDate(EventId id) : base(id)
    {
    }

    internal static ImportantDate Create(EventId id, string title)
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