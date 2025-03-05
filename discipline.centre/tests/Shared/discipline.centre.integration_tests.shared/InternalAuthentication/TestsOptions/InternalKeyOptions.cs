namespace discipline.centre.integration_tests.shared.InternalAuthentication.TestsOptions;

internal sealed record InternalKeyOptions
{
    public string PrivateCertPassword { get; init; } = string.Empty;
    public string PrivateCertPath { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}