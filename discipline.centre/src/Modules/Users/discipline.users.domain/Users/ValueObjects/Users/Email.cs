using discipline.centre.shared.abstractions.SharedKernel;
using discipline.users.domain.Users.Rules.Users;

namespace discipline.users.domain.Users.ValueObjects.Users;

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

    public static Email Create(string value) => new(value);
    
    private Email(string value) => Value = value;

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string email) => Create(email);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}