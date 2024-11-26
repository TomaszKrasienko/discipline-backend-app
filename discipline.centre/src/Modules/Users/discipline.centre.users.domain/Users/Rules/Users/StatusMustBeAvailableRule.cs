using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class StatusMustBeAvailableRule(string value, IEnumerable<string> availableStatuses) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Status.Unavailable",
        $"Status: {value} is unavailable");

    public bool IsBroken()
        => !availableStatuses.Contains(value);
}