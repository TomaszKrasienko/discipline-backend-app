namespace discipline.centre.users.application.Users.DTOs.Endpoints;

public sealed record CreateUserSubscriptionOrderDto(Ulid SubscriptionId, string? SubscriptionOrderFrequency,
    string? PaymentToken);