using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class FirstNameMustBeFrom2To100LengthRule(string firstName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.FirstName.InvalidLength",
        $"First name: {firstName} has invalid length");

    public bool IsBroken()
        => firstName.Length is < 2 or > 100;
}