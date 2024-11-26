namespace discipline.centre.users.application.Users.DTOs;

public sealed record SubscriptionOrderDto
{
    public Ulid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public Ulid SubscriptionId { get; init; }
    public bool StateIsCancelled { get; init; }
    public DateOnly? StateActiveTill { get; init; }
    public DateOnly? Next { get; init; }
    public string? PaymentToken { get; init; }
    public int? Type { get; init; }
}