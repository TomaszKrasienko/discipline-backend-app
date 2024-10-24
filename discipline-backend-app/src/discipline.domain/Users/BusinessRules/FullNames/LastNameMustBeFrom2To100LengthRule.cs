using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules.FullNames;

internal sealed class LastNameMustBeFrom2To100LengthRule(string lastName) : IBusinessRule
{
    public Exception Exception => new InvalidUserLastNameException(lastName);

    public bool IsBroken()
        => lastName.Length is < 2 or > 100;
}