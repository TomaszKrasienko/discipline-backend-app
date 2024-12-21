namespace discipline.centre.shared.infrastructure.Events.Brokers.Configuration;

internal sealed class RedisBrokerOptions
{
    public required string ConnectionString { get; init; }
}