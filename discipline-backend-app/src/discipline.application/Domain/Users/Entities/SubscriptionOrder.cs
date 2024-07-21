using discipline.application.Domain.SharedKernel;

namespace discipline.application.Domain.Users.Entities;

internal sealed class SubscriptionOrder
{
    public EntityId Id { get; }
    public EntityId TypeId { get; }
    public StartDate StartDate { get; private set; }
    public IsRealized IsRealized { get; private set; }
    public CreatedAt CreatedAt { get; set; }

    private SubscriptionOrder(EntityId id, EntityId typeId, CreatedAt createdAt)
    {
        Id = id;
        TypeId = typeId;
        CreatedAt = createdAt;
    }
    
    //For mongo
    internal SubscriptionOrder(EntityId id, EntityId typeId, StartDate startDate, IsRealized isRealized, CreatedAt createdAt)
        : this(id, typeId, createdAt)
    {
        StartDate = startDate;
        IsRealized = isRealized;
    }
}

internal sealed record StartDate(DateOnly Value)
{
    public static implicit operator DateOnly(StartDate startDate)
        => startDate.Value;

    public static implicit operator StartDate(DateOnly value)
        => new StartDate(value);
}

internal sealed record IsRealized(bool Value)
{
    public static implicit operator bool(IsRealized isRealized)
        => isRealized.Value;

    public static implicit operator IsRealized(bool value)
        => new IsRealized(value);
}

internal sealed record CreatedAt(DateTime Value)
{
    public static implicit operator DateTime(CreatedAt createdAt)
        => createdAt.Value;

    public static implicit operator CreatedAt(DateTime value)
        => new CreatedAt(value);
}