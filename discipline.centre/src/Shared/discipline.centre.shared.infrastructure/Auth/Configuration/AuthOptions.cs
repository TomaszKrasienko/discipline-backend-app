namespace discipline.centre.shared.infrastructure.Auth.Configuration;

public sealed record AuthOptions
{
    public string PublicCertPath { get; set; } = string.Empty;
    public string PrivateCertPath { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public TimeSpan Expiry { get; set; }
}