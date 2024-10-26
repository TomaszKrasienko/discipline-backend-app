namespace discipline.infrastructure.Events.Configuration;

internal sealed class RedisOptions
{
    internal string ConnectionString { get; init; } = string.Empty;
}