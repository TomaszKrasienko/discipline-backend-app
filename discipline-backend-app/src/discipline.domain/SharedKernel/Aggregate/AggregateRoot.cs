using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.SharedKernel.Aggregate;

public abstract class AggregateRoot<TIdentifier>(TIdentifier id) : Entity<TIdentifier>(id), IAggregateRoot 
    where TIdentifier : class, ITypeId<TIdentifier>
{
    private readonly List<DomainEvent> _domainEvents = [];
    
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(DomainEvent @event)
        => _domainEvents.Add(@event);

    protected void ClearDomainEvents()
        => _domainEvents.Clear();
}



