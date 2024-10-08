namespace discipline.domain.SharedKernel;

public record EntityId<T>(T Value)
{
    
}

public sealed record EntityId : EntityId<Guid>
{
    public EntityId(Guid value) : base(value)
    {
        
    }

    public EntityId() : this(Guid.NewGuid())
    {
        
    }

    public bool IsEmpty() => Value == Guid.NewGuid();

    public static implicit operator Guid(EntityId entityId)
        => entityId.Value;
    
    public static implicit operator Guid?(EntityId entityId)
        => entityId?.Value;

    public static implicit operator EntityId(Guid value)
        => new EntityId(value);

    public override string ToString()
        => Value.ToString();
} 