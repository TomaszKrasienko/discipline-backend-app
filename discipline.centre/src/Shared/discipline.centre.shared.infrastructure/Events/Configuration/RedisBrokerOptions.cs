namespace discipline.centre.shared.infrastructure.Events.Configuration;

internal sealed class RedisBrokerOptions
{
    internal string ConnectionString { get; init; } = string.Empty;
}