using discipline.domain.SharedKernel;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Exceptions;
using discipline.domain.Users.ValueObjects;

namespace discipline.domain.Users.Entities;

public sealed class User : AggregateRoot<Ulid>
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public Status Status { get; private set; }
    public SubscriptionOrder SubscriptionOrder { get; private set; }

    private User(Ulid id) : base(id)
    {
        
    }

    //For mongo
    public User(Ulid id, Email email, Password password, FullName fullName,
        Status status, SubscriptionOrder subscriptionOrder) : this(id)
    {
        Email = email;
        Password = password;
        FullName = fullName;
        Status = status;
        SubscriptionOrder = subscriptionOrder;
    }

    public static User Create(Ulid id, string email, string password, string firstName, string lastName)
    {
        var user = new User(id);
        user.ChangeEmail(email);
        user.ChangePassword(password);
        user.ChangeFullName(firstName, lastName);
        user.Status = Status.Created();
        return user;
    }

    private void ChangeEmail(string email)
        => Email = email;

    private void ChangePassword(string password)
        => Password = password;

    private void ChangeFullName(string firstName, string lastName)
        => FullName = new FullName(firstName, lastName);

    internal void CreatePaidSubscriptionOrder(Ulid id, Subscription subscription,
        SubscriptionOrderFrequency subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        if (SubscriptionOrder is PaidSubscriptionOrder)
        {
            throw new SubscriptionOrderForUserAlreadyExistsException(Id);
        }
        SubscriptionOrder = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency,
            now, cardNumber, cardCvvNumber);
        Status = Status.PaidSubscriptionPicked();
    }

    internal void CreateFreeSubscriptionOrder(Ulid id, Subscription subscription,
        DateTime now)
    {        
        if (SubscriptionOrder is FreeSubscriptionOrder)
        {
            throw new SubscriptionOrderForUserAlreadyExistsException(Id);
        }
        SubscriptionOrder = FreeSubscriptionOrder.Create(id, subscription, now);
        Status = Status.FreeSubscriptionPicked();
    }

    //Todo: Tests
    public bool IsUserActive()
        => Status == Status.FreeSubscriptionPicked() || Status  == Status.PaidSubscriptionPicked();
}