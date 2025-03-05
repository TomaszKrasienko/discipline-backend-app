namespace discipline.centre.shared.infrastructure.Logging.Configuration.Options;

internal sealed record OpenTelemetryOptions
{
    public string InternalSourceName { get; init; } = string.Empty;
}