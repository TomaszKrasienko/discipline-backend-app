using discipline.application.Domain.Users.Enums;

namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record Type(SubscriptionOrderFrequency Value)
{
    public static implicit operator Type(SubscriptionOrderFrequency value)
        => new(value);

    public static implicit operator SubscriptionOrderFrequency? (Type type)
        => type?.Value;

    public static implicit operator SubscriptionOrderFrequency(Type type)
        => type.Value;
}