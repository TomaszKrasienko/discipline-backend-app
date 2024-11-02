using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.users.domain.Users.ValueObjects.Users;

public sealed class Password(string value) : ValueObject
{
    public string Value { get; } = value;
    
    public static implicit operator Password(string value)
        => new(value);
    
    public static implicit operator string(Password password)
        => password.Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}