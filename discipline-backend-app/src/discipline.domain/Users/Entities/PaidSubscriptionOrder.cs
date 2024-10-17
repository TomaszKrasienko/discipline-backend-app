using discipline.domain.SharedKernel;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;
using Type = discipline.domain.Users.ValueObjects.Type;

namespace discipline.domain.Users.Entities;

public sealed class PaidSubscriptionOrder : SubscriptionOrder
{
    public Next Next { get; private set; }
    public PaymentDetails PaymentDetails { get; private set; }
    public Type Type { get; private set; }

    private PaidSubscriptionOrder(Ulid id, CreatedAt createdAt) : base(id, createdAt)
    {
    }

    //For mongo
    public PaidSubscriptionOrder(Ulid id, Guid subscriptionId, CreatedAt createdAt, State state,
        Next next, PaymentDetails paymentDetails, Type type) : base(id, createdAt, subscriptionId,
        state)
    {
        Next = next;
        PaymentDetails = paymentDetails;
        Type = type;
    }
    
    internal static PaidSubscriptionOrder Create(Ulid id, Subscription subscription, SubscriptionOrderFrequency subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        if (subscription is null)
        {
            throw new NullSubscriptionException();
        }

        if (subscription.IsFreeSubscription())
        {
            throw new InvalidSubscriptionTypeException();
        }

        var subscriptionOrder = new PaidSubscriptionOrder(id, now); 
        subscriptionOrder.ChangeSubscriptionId(subscription.Id);
        subscriptionOrder.ChangeState(false, now, subscriptionOrderFrequency);
        subscriptionOrder.ChangeNext(now, subscriptionOrderFrequency);
        subscriptionOrder.ChangePaymentDetails(cardNumber, cardCvvNumber);
        subscriptionOrder.ChangeType(subscriptionOrderFrequency);
        return subscriptionOrder;
    }

    private void ChangeState(bool isCancelled, DateTime now, SubscriptionOrderFrequency subscriptionOrderFrequency)
    {
        switch (subscriptionOrderFrequency)
        {
            case SubscriptionOrderFrequency.Monthly:
                SetState(new State(false, DateOnly.FromDateTime(now).AddMonths(1).AddDays(-1)));
                break;
            case SubscriptionOrderFrequency.Yearly:
                SetState(new State(false, DateOnly.FromDateTime(now).AddYears(1).AddDays(-1)));
                break;
        }
    }

    private void ChangeNext(DateTime now, SubscriptionOrderFrequency subscriptionOrderFrequency)
    {
        Next = subscriptionOrderFrequency switch
        {
            SubscriptionOrderFrequency.Monthly => DateOnly.FromDateTime(now).AddMonths(1),
            SubscriptionOrderFrequency.Yearly => DateOnly.FromDateTime(now).AddYears(1),
            _ => Next
        };
    }

    private void ChangePaymentDetails(string cardNumber, string cvvNumber)
        => PaymentDetails = new PaymentDetails(cardNumber, cvvNumber);

    private void ChangeType(SubscriptionOrderFrequency subscriptionOrderFrequency)
        => Type = subscriptionOrderFrequency;
}