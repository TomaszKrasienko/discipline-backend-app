using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Statuses;

internal sealed class StatusMustBeAvailableRule(string value, IEnumerable<string> availableStatuses) : IBusinessRule
{
    public Exception Exception => new UnavailableStatusException(value);

    public bool IsBroken()
        => !availableStatuses.Contains(value);
}