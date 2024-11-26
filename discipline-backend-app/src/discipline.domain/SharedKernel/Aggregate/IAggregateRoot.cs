namespace discipline.domain.SharedKernel.Aggregate;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> DomainEvents { get; }
}