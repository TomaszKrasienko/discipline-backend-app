using discipline.domain.SharedKernel;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public abstract class SubscriptionOrder
{
    public EntityId Id { get; }
    public CreatedAt CreatedAt { get; }
    public EntityId SubscriptionId { get; private set; }
    public State State { get; private set; }

    protected SubscriptionOrder(EntityId id, CreatedAt createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }

    protected SubscriptionOrder(EntityId id, CreatedAt createdAt,
        EntityId subscriptionId, State state) : this(id, createdAt)
    {
        SubscriptionId = subscriptionId;
        State = state;
    }

    internal void ChangeSubscriptionId(Guid subscriptionId)
        => SubscriptionId = subscriptionId;

    protected void SetState(State state)
        => State = state;

    // internal void ChangeState(bool isCancelled, DateTime? now, SubscriptionOrderFrequency)
    //     => State = new State(
    //         isCancelled,
    //         now is not null ? DateOnly.FromDateTime(now.Value).AddMonths(1).AddDays(-1) : null);
}