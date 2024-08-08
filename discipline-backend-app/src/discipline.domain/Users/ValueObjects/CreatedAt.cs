using discipline.domain.Users.Exceptions;

namespace discipline.domain.Users.ValueObjects;

public sealed record CreatedAt
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
}