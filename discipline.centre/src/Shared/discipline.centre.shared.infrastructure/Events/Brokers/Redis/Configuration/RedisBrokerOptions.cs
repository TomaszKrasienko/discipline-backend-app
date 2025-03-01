namespace discipline.centre.shared.infrastructure.Events.Brokers.Redis.Configuration;

internal sealed class RedisBrokerOptions
{
    public required string ConnectionString { get; init; }
}