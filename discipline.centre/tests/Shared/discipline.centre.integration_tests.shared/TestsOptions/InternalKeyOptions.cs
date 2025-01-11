namespace discipline.centre.integration_tests.shared.TestsOptions;

internal sealed record InternalKeyOptions
{
    public string? PrivateCertPassword { get; init; }
    public string? PrivateCertPath { get; init; }
}