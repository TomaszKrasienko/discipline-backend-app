using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Statuses;

internal sealed class StatusCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Status.Empty",
        "User status can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}