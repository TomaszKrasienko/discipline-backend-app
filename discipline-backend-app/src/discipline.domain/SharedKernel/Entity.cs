namespace discipline.domain.SharedKernel;

public abstract class Entity<TIdentifier>(TIdentifier id) : IEntity where TIdentifier : struct 
{
    public TIdentifier Id { get; } = id;
    
    public int Version { get; private set; }

    public void IncreaseVersion()
        => Version++;
}