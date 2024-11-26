using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class LastNameMustBeFrom2To100LengthRule(string lastName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.LastName.InvalidLength",
        $"Last name: {lastName} has invalid length");

    public bool IsBroken()
        => lastName.Length is < 2 or > 100;
}