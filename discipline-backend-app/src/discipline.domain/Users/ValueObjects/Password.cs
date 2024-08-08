namespace discipline.domain.Users.ValueObjects;

public sealed record Password(string Value)
{
    public static implicit operator Password(string value)
        => new Password(value);
    
    public static implicit operator string(Password password)
        => password.Value;
}