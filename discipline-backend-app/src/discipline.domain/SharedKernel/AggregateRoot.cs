namespace discipline.domain.SharedKernel;

public abstract class AggregateRoot<T>
{
    public T Id { get; }

    protected AggregateRoot(T id)
        => Id = id;
}

public abstract class AggregateRoot
{
    
}


