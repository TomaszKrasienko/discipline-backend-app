namespace discipline.centre.shared.infrastructure.Auth.Configuration;

public sealed record AuthOptions
{
    public string PublicCertPath { get; init; } = string.Empty;
    public string PrivateCertPath { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public TimeSpan TokenExpiry { get; init; }
    public int RefreshTokenLength { get; init; }
    public TimeSpan RefreshTokenExpiry { get; init; }
}