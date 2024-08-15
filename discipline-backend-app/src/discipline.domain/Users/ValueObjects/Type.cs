using discipline.domain.Users.Enums;

namespace discipline.domain.Users.ValueObjects;

public sealed record Type(SubscriptionOrderFrequency Value)
{
    public static implicit operator Type(SubscriptionOrderFrequency value)
        => new(value);

    public static implicit operator SubscriptionOrderFrequency? (Type type)
        => type?.Value;

    public static implicit operator SubscriptionOrderFrequency(Type type)
        => type.Value;
}