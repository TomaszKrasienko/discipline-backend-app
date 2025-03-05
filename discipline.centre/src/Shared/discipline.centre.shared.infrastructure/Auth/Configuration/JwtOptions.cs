namespace discipline.centre.shared.infrastructure.Auth.Configuration;

internal sealed record JwtOptions
{
    public required Dictionary<string, AuthorizeParamsOptions> AuthorizeParams { get; set; }
}

internal sealed record AuthorizeParamsOptions
{
    public required string PublicCertPath { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}

