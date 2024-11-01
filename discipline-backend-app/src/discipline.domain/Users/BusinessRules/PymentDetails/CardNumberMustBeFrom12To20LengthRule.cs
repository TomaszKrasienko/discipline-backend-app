using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.PymentDetails;

internal sealed class CardNumberMustBeFrom12To20LengthRule(string cardNumber) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.PaymentDetails.CardNumber.InvalidLength",
        $"Value: {cardNumber} has invalid length");

    public bool IsBroken()
        => cardNumber.Length is <= 12 or >= 20;
}