using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.Emails;

internal sealed class EmailCanNotBeEmptyRule(string email) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Email.Empty", 
        "Email can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(email);
}