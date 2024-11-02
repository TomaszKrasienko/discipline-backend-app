using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class CardNumberMustBeFrom12To20LengthRule(string cardNumber) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.PaymentDetails.CardNumber.InvalidLength",
        $"Value: {cardNumber} has invalid length");

    public bool IsBroken()
        => cardNumber.Length is <= 12 or >= 20;
}