namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed record KeyPublishingOptions
{
    public required string PrivateCertPath { get; init; }
    public required string PrivateCertPassword { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public TimeSpan TokenExpiry { get; init; }
}