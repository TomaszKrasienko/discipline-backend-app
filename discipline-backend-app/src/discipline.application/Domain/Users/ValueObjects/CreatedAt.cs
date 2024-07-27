using discipline.application.Domain.Users.Exceptions;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record CreatedAt
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