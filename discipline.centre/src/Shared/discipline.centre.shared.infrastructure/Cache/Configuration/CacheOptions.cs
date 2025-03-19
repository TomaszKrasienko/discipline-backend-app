namespace discipline.centre.shared.infrastructure.Cache.Configuration;

public sealed record CacheOptions
{
    public TimeSpan Expire { get; init; }
}