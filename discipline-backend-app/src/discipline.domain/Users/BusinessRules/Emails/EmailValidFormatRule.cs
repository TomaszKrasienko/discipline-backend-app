using System.Text.RegularExpressions;
using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.BusinessRules;

internal sealed class EmailValidFormatRule(string email) : IBusinessRule
{
    private static readonly Regex Regex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.Compiled);
    
    public Exception Exception => new InvalidUserEmailException(email);

    public bool IsBroken()
        => !Regex.IsMatch(email);
}