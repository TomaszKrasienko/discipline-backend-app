using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.CreatedAt;

internal sealed class CreatedAtCanNoBeDefaultRule(DateTimeOffset value) : IBusinessRule
{
    public Exception Exception => new DomainException("SubscriptionOrder.CreatedAt.Default",
        "Created at can not be default value");

    public bool IsBroken()
        => value == default;
}