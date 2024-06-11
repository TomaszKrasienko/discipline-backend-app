namespace discipline.application.Domain.ValueObjects.SharedKernel;

public abstract class AggregateRoot
{
}

public abstract class AggregateRoot<T>
{
    public T Id { get; protected set; }
}
