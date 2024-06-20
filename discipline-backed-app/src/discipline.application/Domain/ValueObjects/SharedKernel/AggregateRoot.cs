namespace discipline.application.Domain.ValueObjects.SharedKernel;

internal abstract class AggregateRoot
{
    internal EntityId Id { get; set; }
}
