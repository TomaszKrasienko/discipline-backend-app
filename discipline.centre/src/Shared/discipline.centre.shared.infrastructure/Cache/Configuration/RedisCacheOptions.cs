namespace discipline.centre.shared.infrastructure.Cache.Configuration;

public sealed record RedisCacheOptions
{
    public string ConnectionString { get; init; } = string.Empty;
}