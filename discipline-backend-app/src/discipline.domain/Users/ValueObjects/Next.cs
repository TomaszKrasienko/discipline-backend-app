using discipline.domain.SharedKernel;

namespace discipline.domain.Users.ValueObjects;

public sealed class Next(DateOnly value) : ValueObject
{
    public DateOnly Value { get; set; } = value;
    
    public static implicit operator DateOnly(Next next)
        => next.Value;
    
    public static implicit operator Next(DateOnly value)
        => new Next(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}