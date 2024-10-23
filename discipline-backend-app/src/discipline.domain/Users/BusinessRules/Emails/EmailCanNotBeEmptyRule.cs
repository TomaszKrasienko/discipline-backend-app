using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules;

internal sealed class EmailCanNotBeEmptyRule(string email) : IBusinessRule
{
    public Exception Exception => new EmptyUserEmailException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(email);
}