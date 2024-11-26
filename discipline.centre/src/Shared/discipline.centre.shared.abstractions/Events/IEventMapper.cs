using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.abstractions.Events;

public interface IEventMapper
{
    IEvent MapAsEvent(DomainEvent domainEvent);
}