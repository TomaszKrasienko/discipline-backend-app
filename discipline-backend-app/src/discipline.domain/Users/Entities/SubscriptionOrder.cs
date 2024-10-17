using discipline.domain.SharedKernel;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public abstract class SubscriptionOrder : Entity<Ulid>
{
    public CreatedAt CreatedAt { get; }
    public Guid SubscriptionId { get; private set; }
    public State State { get; private set; }

    protected SubscriptionOrder(Ulid id, CreatedAt createdAt) : base(id)
        => CreatedAt = createdAt;
    
    protected SubscriptionOrder(Ulid id, CreatedAt createdAt,
        Guid subscriptionId, State state) : this(id, createdAt)
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