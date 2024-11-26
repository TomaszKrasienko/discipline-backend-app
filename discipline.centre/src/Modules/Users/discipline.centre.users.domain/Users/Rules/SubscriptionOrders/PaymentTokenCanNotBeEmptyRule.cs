using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class PaymentTokenCanNotBeEmptyRule(string token) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.PaymentDetails.Token.CanNotBeEmpty",
        "Payment token can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(token);
}