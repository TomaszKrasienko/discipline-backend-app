using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Statuses;

internal sealed class StatusCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new EmptyStatusException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}