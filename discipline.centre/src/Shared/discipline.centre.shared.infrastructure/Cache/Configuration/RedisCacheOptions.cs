namespace discipline.centre.shared.infrastructure.Cache.Configuration;

public sealed record RedisCacheOptions
{
    public required string ConnectionString { get; init; }
    
}