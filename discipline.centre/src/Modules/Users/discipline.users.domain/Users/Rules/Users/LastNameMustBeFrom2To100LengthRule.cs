using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.users.domain.Users.Rules.Users;

internal sealed class LastNameMustBeFrom2To100LengthRule(string lastName) : IBusinessRule
{
    public Exception Exception => new DomainException("User.FullName.LastName.InvalidLength",
        $"Last name: {lastName} has invalid length");

    public bool IsBroken()
        => lastName.Length is < 2 or > 100;
}