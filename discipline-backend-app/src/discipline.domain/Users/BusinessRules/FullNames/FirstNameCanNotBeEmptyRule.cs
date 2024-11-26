using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class FirstNameCanNotBeEmptyRule(string firstName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.FirstName.Empty",
        "First name can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(firstName);
}