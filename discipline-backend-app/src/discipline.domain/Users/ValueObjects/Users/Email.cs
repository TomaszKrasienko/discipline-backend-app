using System.Text.RegularExpressions;
using discipline.domain.SharedKernel;
using discipline.domain.Users.BusinessRules;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects.Users;

public sealed class Email : ValueObject
{
    private readonly string _value = null!;
    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new EmailCanNotBeEmptyRule(value));
            CheckRule(new EmailValidFormatRule(value));
            _value = value;
        }
    }

    private Email(string value)
        => Value = value;

    public static implicit operator string(Email email)
        => email.Value;

    public static implicit operator Email(string email)
        => new (email);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}