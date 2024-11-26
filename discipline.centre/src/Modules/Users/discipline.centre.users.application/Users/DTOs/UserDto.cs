namespace discipline.centre.users.application.Users.DTOs;

public sealed record UserDto
{
    public Ulid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Status { get; init; }
    public SubscriptionOrderDto? SubscriptionOrder { get; init; }
}