using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.CreatedAt;

internal sealed class CreatedAtCanNoBeDefaultRule(DateTimeOffset value) : IBusinessRule
{
    public Exception Exception => new DefaultCreatedAtException();

    public bool IsBroken()
        => value == default;
}