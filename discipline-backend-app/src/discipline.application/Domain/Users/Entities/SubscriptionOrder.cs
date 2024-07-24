using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;
using Type = discipline.application.Domain.Users.ValueObjects.Type;

namespace discipline.application.Domain.Users.Entities;

internal sealed class SubscriptionOrder
{
    public EntityId Id { get; }
    public CreatedAt CreatedAt { get; }
    public EntityId SubscriptionId { get; private set; }
    public State State { get; private set; }
    public Next Next { get; private set; }
    public PaymentDetails PaymentDetails { get; private set; }
    public Type Type { get; private set; }

    private SubscriptionOrder(EntityId id, CreatedAt createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
    
    //For mongo
    public SubscriptionOrder(EntityId id, EntityId subscriptionId, CreatedAt createdAt, State state,
        Next next, PaymentDetails paymentDetails, Type type) : this(id, createdAt)
    {
        SubscriptionId = subscriptionId;
        State = state;
        Next = next;
        PaymentDetails = paymentDetails;
        Type = type;
    }

    internal static SubscriptionOrder Create(Guid id, Subscription subscription, SubscriptionOrderFrequency? subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        if (subscription is null)
        {
            throw new NullSubscriptionException();
        }

        if (subscription.IsFreeSubscription() && subscriptionOrderFrequency is null)
        {
            throw new NullSubscriptionOrderFrequencyException(subscription.Id);
        }

        var subscriptionOrder = new SubscriptionOrder(id, now); 
        subscriptionOrder.ChangeState(now);
        subscriptionOrder.ChangeNext(now);
        subscriptionOrder.ChangePaymentDetails(cardNumber, cardCvvNumber);
        subscriptionOrder.ChangeType(subscriptionOrderFrequency);
        return subscriptionOrder;
    }

    private void ChangeState(DateTime now)
        => State = new State(false, DateOnly.FromDateTime(now).AddMonths(1).AddDays(-1));

    private void ChangeNext(DateTime now)
        => Next = DateOnly.FromDateTime(now).AddMonths(1);

    private void ChangePaymentDetails(string cardNumber, string cvvNumber)
        => PaymentDetails = new PaymentDetails(cardNumber, cvvNumber);

    private void ChangeType(SubscriptionOrderFrequency? subscriptionOrderFrequency)
        => Type = subscriptionOrderFrequency;
}