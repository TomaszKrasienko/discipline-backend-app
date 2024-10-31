using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class LastNameCanNotBeEmptyRule(string lastName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.LastName.Empty",
        "Last name can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(lastName);
}