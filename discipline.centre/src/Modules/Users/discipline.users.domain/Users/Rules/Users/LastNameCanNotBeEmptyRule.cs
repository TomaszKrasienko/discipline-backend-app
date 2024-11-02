using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.Users;

internal sealed class LastNameCanNotBeEmptyRule(string lastName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.LastName.Empty",
        "Last name can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(lastName);
}