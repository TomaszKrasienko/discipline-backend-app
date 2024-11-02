using System.Text.RegularExpressions;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.users.domain.Users.Rules.Users;

internal sealed class PasswordMustContainsCharactersRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("User.Password.SpecialCharacters",
        "Password have to contain at least one number, one special character, one upper and lower characters");

    public bool IsBroken()
    {
        var hasNumber = Regex.IsMatch(value, @"\d"); 
        var hasSpecialChar = Regex.IsMatch(value, @"[!@#$%^&*()?:{}|<>]");
        var hasUpperCase = Regex.IsMatch(value, @"[A-Z]");
        var hasLowerCase = Regex.IsMatch(value, @"[a-z]");
        return !(hasNumber && hasSpecialChar && hasUpperCase && hasLowerCase);
    }
}