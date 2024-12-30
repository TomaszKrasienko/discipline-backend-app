namespace discipline.centre.shared.infrastructure.Logging.Configuration.Options;

internal sealed record JaegerOptions
{
    public string Endpoint { get; init; } = string.Empty;
}