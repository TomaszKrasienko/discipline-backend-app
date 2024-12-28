namespace discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;

internal sealed record KeyPublishingOptions
{
    public string PrivateCertPath { get; init; } = string.Empty;
    public string PrivateCertPassword { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public TimeSpan TokenExpiry { get; init; }
}