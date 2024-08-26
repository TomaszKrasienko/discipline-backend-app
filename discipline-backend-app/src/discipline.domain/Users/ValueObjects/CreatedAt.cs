using discipline.domain.SharedKernel;
using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed class CreatedAt : ValueObject
{
    public DateTime Value { get; }

    public CreatedAt(DateTime value)
    {
        if (value == default)
        {
            throw new DefaultCreatedAtException();
        }
        Value = value;
    }

    public static implicit operator DateTime(CreatedAt createdAt)
        => createdAt.Value;

    public static implicit operator CreatedAt(DateTime value)
        => new CreatedAt(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}