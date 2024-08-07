namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record Password(string Value)
{
    public static implicit operator Password(string value)
        => new Password(value);
    
    public static implicit operator string(Password password)
        => password.Value;
}