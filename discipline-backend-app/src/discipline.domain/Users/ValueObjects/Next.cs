namespace discipline.domain.Users.ValueObjects;

public sealed record Next(DateOnly Value)
{
    public static implicit operator DateOnly(Next next)
        => next.Value;
    
    public static implicit operator Next(DateOnly value)
        => new Next(value);
}