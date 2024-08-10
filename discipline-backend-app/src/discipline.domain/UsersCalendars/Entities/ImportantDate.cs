using discipline.domain.SharedKernel;
using discipline.domain.UsersCalendars.ValueObjects.Event;

namespace discipline.domain.UsersCalendars.Entities;

public sealed class ImportantDate : Event
{
    //For mongo
    public ImportantDate(EntityId id, Title title) : base(id, title)
    {
    }

    internal static ImportantDate Create(Guid id, string title)
        => new ImportantDate(id, title);
        
}