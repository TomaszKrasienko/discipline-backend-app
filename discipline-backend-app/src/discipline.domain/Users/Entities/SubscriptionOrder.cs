using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public abstract class SubscriptionOrder : Entity<SubscriptionOrderId>
{
    public CreatedAt CreatedAt { get; }
    public SubscriptionId SubscriptionId { get; private set; }
    public State State { get; private set; }

    protected SubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt) : base(id)
        => CreatedAt = createdAt;
    
    protected SubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt,
        SubscriptionId subscriptionId, State state) : this(id, createdAt)
    {
        SubscriptionId = subscriptionId;
        State = state;
    }

    internal void ChangeSubscriptionId(SubscriptionId subscriptionId)
        => SubscriptionId = subscriptionId;

    protected void SetState(State state)
        => State = state;

    // internal void ChangeState(bool isCancelled, DateTime? now, SubscriptionOrderFrequency)
    //     => State = new State(
    //         isCancelled,
    //         now is not null ? DateOnly.FromDateTime(now.Value).AddMonths(1).AddDays(-1) : null);
}