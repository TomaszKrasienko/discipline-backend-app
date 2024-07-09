using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.UsersCalendars.ValueObjects.Event;

namespace discipline.application.Domain.UsersCalendars.Entities;

internal sealed class ImportantDate : Event
{
    private ImportantDate(EntityId id, Title title) : base(id, title)
    {
    }

    internal static ImportantDate Create(Guid id, string title)
        => new ImportantDate(id, title);
        
}