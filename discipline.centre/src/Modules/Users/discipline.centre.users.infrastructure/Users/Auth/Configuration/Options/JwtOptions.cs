namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed record JwtOptions
{
    public KeyPublishingOptions KeyPublishing { get; init; } = new ();
}