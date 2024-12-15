namespace discipline.centre.shared.infrastructure.Events.Brokers.Configuration;

internal sealed class RedisBrokerOptions
{
    internal string ConnectionString { get; init; } = string.Empty;
}