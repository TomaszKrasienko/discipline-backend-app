namespace discipline.infrastructure.Auth.Configuration;

internal sealed class AuthOptions
{
    internal string PublicCertPath { get; init; } = string.Empty;
    internal string PrivateCertPath { get; init; } = string.Empty;
    internal string Password { get; init; } = string.Empty;
    internal string Issuer { get; init; } = string.Empty;
    internal string Audience { get; init; } = string.Empty;
    internal TimeSpan Expiry { get; init; }
}