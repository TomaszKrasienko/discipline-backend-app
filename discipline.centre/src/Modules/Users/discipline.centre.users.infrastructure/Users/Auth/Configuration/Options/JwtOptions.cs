namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed record JwtOptions
{
    public required KeyPublishingOptions KeyPublishing { get; init; }
}