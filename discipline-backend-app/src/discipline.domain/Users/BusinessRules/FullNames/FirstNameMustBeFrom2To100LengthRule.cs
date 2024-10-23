using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class FirstNameMustBeFrom2To100LengthRule(string firstName) : IBusinessRule
{
    public Exception Exception => new InvalidUserFirstNameException(firstName);

    public bool IsBroken()
        => firstName.Length is < 2 or > 100;
}