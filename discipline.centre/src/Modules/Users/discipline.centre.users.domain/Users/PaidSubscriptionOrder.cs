using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.ValueObjects;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.Rules.SubscriptionOrders;
using discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders;
using Type = discipline.centre.users.domain.Users.ValueObjects.SubscriptionOrders.Type;

namespace discipline.centre.users.domain.Users;

using Type = ValueObjects.SubscriptionOrders.Type;

public sealed class PaidSubscriptionOrder : SubscriptionOrder
{
    public Next Next { get; private set; }
    public PaymentDetails PaymentDetails { get; private set; }
    public Type Type { get; private set; }
    
    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subscriptionId"></param>
    /// <param name="createdAt"></param>
    /// <param name="state"></param>
    /// <param name="next"></param>
    /// <param name="paymentDetails"></param>
    /// <param name="type"></param>
    public PaidSubscriptionOrder(SubscriptionOrderId id, SubscriptionId subscriptionId, CreatedAt createdAt, State state,
        Next next, PaymentDetails paymentDetails, Type type) : base(id, createdAt, subscriptionId,
        state)
    {
        Next = next;
        PaymentDetails = paymentDetails;
        Type = type;
    }
    
    internal static PaidSubscriptionOrder Create(SubscriptionOrderId id, Subscription subscription, SubscriptionOrderFrequency subscriptionOrderFrequency, DateTimeOffset now,
        string paymentToken)
    {
        CheckRule(new SubscriptionMustBeValidTypeRule(typeof(PaidSubscriptionOrder), subscription));

        var state = GetState(false, now, subscriptionOrderFrequency);
        var next = GetNext(now, subscriptionOrderFrequency);
        var paymentDetails = PaymentDetails.Create(paymentToken);
        return new PaidSubscriptionOrder(id, subscription.Id, now, state,
            next, paymentDetails, subscriptionOrderFrequency); 
    }

    private static State GetState(bool isCancelled, DateTimeOffset now, SubscriptionOrderFrequency subscriptionOrderFrequency) =>
        subscriptionOrderFrequency switch
        {
            SubscriptionOrderFrequency.Monthly => new State(isCancelled,
                DateOnly.FromDateTime(now.Date).AddMonths(1).AddDays(-1)),
            SubscriptionOrderFrequency.Yearly => new State(isCancelled,
                DateOnly.FromDateTime(now.Date).AddYears(1).AddDays(-1)),
            _ => throw new ArgumentException($"Frequency: {subscriptionOrderFrequency} is invalid")
        };

    private static DateOnly GetNext(DateTimeOffset now, SubscriptionOrderFrequency subscriptionOrderFrequency) =>
        subscriptionOrderFrequency switch
        {
            SubscriptionOrderFrequency.Monthly => DateOnly.FromDateTime(now.DateTime).AddMonths(1),
            SubscriptionOrderFrequency.Yearly => DateOnly.FromDateTime(now.DateTime).AddYears(1),
            _ => throw new ArgumentException($"Frequency: {subscriptionOrderFrequency} is invalid")
        };
}