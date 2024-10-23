using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.BusinessRules.SubscriptionOrders;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Events;
using discipline.domain.Users.ValueObjects;
using discipline.domain.Users.ValueObjects.Users;

namespace discipline.domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    private SubscriptionOrder? _subscriptionOrder;
    public Email Email { get; }
    public Password Password { get; }
    public FullName FullName { get; }
    public Status Status { get; private set; }
    public SubscriptionOrder? SubscriptionOrder
    {
        get => _subscriptionOrder;
        private set
        {
            CheckRule(new SubscriptionOrderAlreadyPickedRule(Id, _subscriptionOrder, value!));
            _subscriptionOrder = value;
        }
    }

    private User(UserId id, Email email, Password password, FullName fullName,
        Status status) : base(id)
    {
        Email = email;
        Password = password;
        FullName = fullName;
        Status = status;
        
        var @event = new UserCreated(id, email);
        AddDomainEvent(@event);
    }
    
    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    public User(UserId id, Email email, Password password, FullName fullName,
        Status status, SubscriptionOrder? subscriptionOrder) : this(id, email, password, fullName,
        status)
    {
        SubscriptionOrder = subscriptionOrder;
    }

    public static User Create(UserId id, string email, string password, string firstName, string lastName)
    {
        var user = new User(id, email, password,  new FullName(firstName, lastName), 
            Status.Created);
        return user;
    }

    internal void CreatePaidSubscriptionOrder(SubscriptionOrderId id, Subscription subscription,
        SubscriptionOrderFrequency subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        SubscriptionOrder = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency,
            now, cardNumber, cardCvvNumber);
        Status = Status.PaidSubscriptionPicked;
    }

    internal void CreateFreeSubscriptionOrder(SubscriptionOrderId id, Subscription subscription,
        DateTime now)
    {     
        SubscriptionOrder = FreeSubscriptionOrder.Create(id, subscription, now);
        Status = Status.FreeSubscriptionPicked;
    }

    //Todo: Tests
    public bool IsUserActive()
        => Status == Status.FreeSubscriptionPicked || Status  == Status.PaidSubscriptionPicked;
}