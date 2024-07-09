namespace discipline.application.Domain.SharedKernel;

internal abstract class AggregateRoot<T>
{
    public T Id { get; }

    protected AggregateRoot(T id)
        => Id = id;
}

internal abstract class AggregateRoot
{
    
}


