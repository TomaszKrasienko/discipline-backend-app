namespace discipline.centre.shared.abstractions.SharedKernel.Aggregate;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> DomainEvents { get; }
}