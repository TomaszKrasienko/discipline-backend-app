using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class CvvLengthMustBe3Rule(string cvvCode) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.PaymentDetails.CvvCode.InvalidLength",
        $"Value: {cvvCode} has invalid length");

    public bool IsBroken()
        => cvvCode.Length is not 3;
}