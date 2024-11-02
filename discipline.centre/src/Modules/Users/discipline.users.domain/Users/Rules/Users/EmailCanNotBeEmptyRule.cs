using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.Users;

internal sealed class EmailCanNotBeEmptyRule(string email) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Email.Empty", 
        "Email can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(email);
}