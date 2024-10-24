using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.BusinessRules.SubscriptionOrders;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;
using Type = discipline.domain.Users.ValueObjects.Type;

namespace discipline.domain.Users.Entities;

public sealed class PaidSubscriptionOrder : SubscriptionOrder
{
    public Next Next { get; private set; }
    public PaymentDetails PaymentDetails { get; private set; }
    public Type Type { get; private set; }
    
    public PaidSubscriptionOrder(SubscriptionOrderId id, SubscriptionId subscriptionId, CreatedAt createdAt, State state,
        Next next, PaymentDetails paymentDetails, Type type) : base(id, createdAt, subscriptionId,
        state)
    {
        Next = next;
        PaymentDetails = paymentDetails;
        Type = type;
    }
    
    internal static PaidSubscriptionOrder Create(SubscriptionOrderId id, Subscription subscription, SubscriptionOrderFrequency subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        CheckRule(new SubscriptionMustBeValidTypeRule(typeof(PaidSubscriptionOrder), subscription));

        var state = GetState(false, now, subscriptionOrderFrequency);
        var next = GetNext(now, subscriptionOrderFrequency);
        var paymentDetails = new PaymentDetails(cardNumber, cardCvvNumber);
        return new PaidSubscriptionOrder(id, subscription.Id, now, state,
            next, paymentDetails, subscriptionOrderFrequency); 
    }

    private static State GetState(bool isCancelled, DateTime now, SubscriptionOrderFrequency subscriptionOrderFrequency) =>
        subscriptionOrderFrequency switch
        {
            SubscriptionOrderFrequency.Monthly => new State(isCancelled,
                DateOnly.FromDateTime(now).AddMonths(1).AddDays(-1)),
            SubscriptionOrderFrequency.Yearly => new State(isCancelled,
                DateOnly.FromDateTime(now).AddYears(1).AddDays(-1)),
            _ => throw new InvalidFrequencyException(subscriptionOrderFrequency)
        };

    private static DateOnly GetNext(DateTime now, SubscriptionOrderFrequency subscriptionOrderFrequency) =>
        subscriptionOrderFrequency switch
        {
            SubscriptionOrderFrequency.Monthly => DateOnly.FromDateTime(now).AddMonths(1),
            SubscriptionOrderFrequency.Yearly => DateOnly.FromDateTime(now).AddYears(1),
            _ => throw new InvalidFrequencyException(subscriptionOrderFrequency)
        };
}