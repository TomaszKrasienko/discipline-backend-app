using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.shared.abstractions.SharedKernel.Aggregate;

public abstract class AggregateRoot<TIdentifier, TValue>(TIdentifier id) : Entity<TIdentifier, TValue>(id), IAggregateRoot 
    where TIdentifier : class, ITypeId<TIdentifier, TValue>
    where TValue : struct
{
    private readonly List<DomainEvent> _domainEvents = [];
    
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    protected void AddDomainEvent(DomainEvent @event)
        => _domainEvents.Add(@event);

    protected void ClearDomainEvents()
        => _domainEvents.Clear();
}



