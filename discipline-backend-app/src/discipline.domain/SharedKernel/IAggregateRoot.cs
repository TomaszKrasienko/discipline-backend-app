namespace discipline.domain.SharedKernel;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> DomainEvents { get; }
}