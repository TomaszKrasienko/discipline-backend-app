using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.SharedKernel;

public abstract class Entity<TIdentifier>(TIdentifier id) : IEntity 
    where TIdentifier : class, ITypeId<TIdentifier>
{
    public TIdentifier Id { get; } = id;
    
    public int Version { get; private set; }

    public void IncreaseVersion()
        => Version++;
    
    protected static void CheckRule(IBusinessRule businessRule)
    {
        if (businessRule.IsBroken())
        {
            throw businessRule.Exception;
        }
    }
}