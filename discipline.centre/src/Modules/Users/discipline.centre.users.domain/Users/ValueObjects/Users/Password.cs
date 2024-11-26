using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Users.Rules.Users;

namespace discipline.centre.users.domain.Users.ValueObjects.Users;

public sealed class Password : ValueObject
{
    private readonly string? _value;
    
    public string? Value
    {
        get => _value;
        private init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            CheckRule(new PasswordMustBeAtLeast8Length(value));
            CheckRule(new PasswordMustContainsCharactersRule(value));
            _value = value;
        }
    }

    public string? HashedValue { get; }

    private Password(string? value, string? hashedValue)
    {
        Value = value;
        HashedValue = hashedValue;
    }

    public static Password Create(string value) => new (value, null);

    public static Password CreateHashed(string hashedValue) => new(null, hashedValue);

    protected override IEnumerable<object> GetAtomicValues()
    {
        if (Value != null) yield return Value;
        if (HashedValue != null) yield return HashedValue;
    }
}