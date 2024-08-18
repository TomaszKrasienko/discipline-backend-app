using discipline.domain.SharedKernel;

namespace discipline.domain.Users.ValueObjects;

public sealed class Password(string value) : ValueObject
{
    public string Value { get; } = value;
    
    public static implicit operator Password(string value)
        => new Password(value);
    
    public static implicit operator string(Password password)
        => password.Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}