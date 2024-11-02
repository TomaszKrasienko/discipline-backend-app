using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.SubscriptionOrders;

internal sealed class CreatedAtCanNoBeDefaultRule(DateTimeOffset value) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.CreatedAt.Default",
        "Created at can not be default value");

    public bool IsBroken()
        => value == default;
}