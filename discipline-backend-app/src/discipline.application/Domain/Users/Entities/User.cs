using discipline.application.Domain.SharedKernel;
using discipline.application.Domain.Users.Enums;
using discipline.application.Domain.Users.Exceptions;
using discipline.application.Domain.Users.ValueObjects;

namespace discipline.application.Domain.Users.Entities;

internal sealed class User : AggregateRoot
{
    public EntityId Id { get; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public SubscriptionOrder SubscriptionOrder { get; private set; }

    //For mongo
    internal User(EntityId id, Email email, Password password, FullName fullName)
    {
        Id = id;
        Email = email;
        Password = password;
        FullName = fullName;
    }

    private User(EntityId id)
        => Id = id;

    internal static User Create(Guid id, string email, string password, string firstName, string lastName)
    {
        var user = new User(id);
        user.ChangeEmail(email);
        user.ChangePassword(password);
        user.ChangeFullName(firstName, lastName);
        return user;
    }

    private void ChangeEmail(string email)
        => Email = email;

    private void ChangePassword(string password)
        => Password = password;

    private void ChangeFullName(string firstName, string lastName)
        => FullName = new FullName(firstName, lastName);

    internal void CreatePaidSubscriptionOrder(Guid id, Subscription subscription,
        SubscriptionOrderFrequency subscriptionOrderFrequency, DateTime now,
        string cardNumber, string cardCvvNumber)
    {
        if (SubscriptionOrder is PaidSubscriptionOrder)
        {
            throw new SubscriptionOrderForUserAlreadyExistsException(Id);
        }
        SubscriptionOrder = PaidSubscriptionOrder.Create(id, subscription, subscriptionOrderFrequency,
            now, cardNumber, cardCvvNumber);
    }
}