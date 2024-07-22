using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class SubscriptionOrder
{
    public EntityId Id { get; }
    public CreatedAt CreatedAt { get; }
    public EntityId SubscriptionId { get; private set; }
    public State State { get; set; }
    public Next Next { get; set; }
    public PaymentDetails PaymentDetails { get; set; }

    private SubscriptionOrder(EntityId id, CreatedAt createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
    
    //For mongo
    public SubscriptionOrder(EntityId id, EntityId subscriptionId, CreatedAt createdAt, State state,
        Next next, PaymentDetails paymentDetails) : this(id, createdAt)
    {
        SubscriptionId = subscriptionId;
        State = state;
        Next = next;
        PaymentDetails = paymentDetails;
    }

    internal static SubscriptionOrder Create(Guid id, Subscription subscription, SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        if (subscription is null)
        {
            throw new NullSubscriptionException();
        }

        if (!subscription.IsFreeSubscription() && subscriptionOrderFrequency is null)
        {
            throw new NullSubscriptionOrderFrequencyException(subscription.Id);
        }

        var subscriptionOrder = new SubscriptionOrder(id, now);
        return subscriptionOrder;
    }

    private void ChangeNext(DateOnly nextDate)
        => Next = nextDate;
}