using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.users.domain.Subscriptions.ValueObjects;
using discipline.users.domain.Users.ValueObjects.SubscriptionOrders;

namespace discipline.users.domain.Users;

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