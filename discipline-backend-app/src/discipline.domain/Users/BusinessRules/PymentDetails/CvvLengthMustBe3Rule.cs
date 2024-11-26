using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.PymentDetails;

internal sealed class CvvLengthMustBe3Rule(string cvvCode) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.PaymentDetails.CvvCode.InvalidLength",
        $"Value: {cvvCode} has invalid length");

    public bool IsBroken()
        => cvvCode.Length is not 3;
}