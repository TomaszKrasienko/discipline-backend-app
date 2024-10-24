using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;

namespace discipline.domain.Users.Entities;

public abstract class SubscriptionOrder : Entity<SubscriptionOrderId>
{
    public CreatedAt CreatedAt { get; private set; }
    public SubscriptionId SubscriptionId { get; private set; }
    public State State { get; private set; }
    
    protected SubscriptionOrder(SubscriptionOrderId id, CreatedAt createdAt,
        SubscriptionId subscriptionId, State state) : base(id)
    {
        CreatedAt = createdAt;
        SubscriptionId = subscriptionId;
        State = state;
    }
}