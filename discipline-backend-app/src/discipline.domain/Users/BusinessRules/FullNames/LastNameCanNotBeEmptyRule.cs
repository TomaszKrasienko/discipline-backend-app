using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class LastNameCanNotBeEmptyRule(string lastName) : IBusinessRule
{
    public Exception Exception => new EmptyUserLastNameException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(lastName);
}