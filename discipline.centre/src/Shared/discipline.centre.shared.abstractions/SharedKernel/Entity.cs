using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.shared.abstractions.SharedKernel;

public abstract class Entity<TIdentifier, TValue>(TIdentifier id) : IEntity 
    where TIdentifier : class, ITypeId<TIdentifier, TValue>
    where TValue : struct
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