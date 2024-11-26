using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Statuses;

internal sealed class StatusMustBeAvailableRule(string value, IEnumerable<string> availableStatuses) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Status.Unavailable",
        $"Status: {value} is unavailable");

    public bool IsBroken()
        => !availableStatuses.Contains(value);
}