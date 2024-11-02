using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class StatusCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Status.Empty",
        "User status can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}