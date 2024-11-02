using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class PasswordMustBeAtLeast8Length(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Password.InvalidLength",
        "Provided password is too short. Password must be at least 8 length");

    public bool IsBroken()
        => value.Length < 8;
}