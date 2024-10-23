using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class FirstNameCanNotBeEmptyRule(string firstName) : IBusinessRule
{
    public Exception Exception => new EmptyUserFirstNameException();

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(firstName);
}