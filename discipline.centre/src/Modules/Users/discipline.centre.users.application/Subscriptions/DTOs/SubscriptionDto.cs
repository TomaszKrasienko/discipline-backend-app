namespace discipline.centre.users.application.Subscriptions.DTOs;

public sealed record SubscriptionDto
{
    public Ulid Id { get; init; }
    public required string Title { get; init; }
    public decimal PricePerMonth { get; init; }
    public decimal PricePerYear { get; init; }
    public bool IsPaid { get; init; }
    public required List<string> Features { get; init; }
}