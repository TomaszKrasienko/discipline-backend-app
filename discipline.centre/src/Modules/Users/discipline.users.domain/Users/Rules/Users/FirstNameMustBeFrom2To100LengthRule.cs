using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.Users;

internal sealed class FirstNameMustBeFrom2To100LengthRule(string firstName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.FirstName.InvalidLength",
        $"First name: {firstName} has invalid length");

    public bool IsBroken()
        => firstName.Length is < 2 or > 100;
}