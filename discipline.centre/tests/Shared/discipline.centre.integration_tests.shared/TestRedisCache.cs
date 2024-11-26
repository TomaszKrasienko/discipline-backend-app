using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace discipline.centre.integration_tests.shared;

public sealed class TestRedisCache : IDisposable
{
    private RedisContainer? _redisContainer;
    private IConnectionMultiplexer _connectionMultiplexer;
    public string ConnectionString { get; }
    
    public TestRedisCache()
    {
        CreateContainer();
        _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisContainer!.GetConnectionString());
        ConnectionString = _redisContainer.GetConnectionString();
    }

    private void CreateContainer()
    {
        _redisContainer = new RedisBuilder()
            .Build();
        _redisContainer.StartAsync().GetAwaiter().GetResult();
    }

    public T? GetValueAsync<T>(string key) where T : class
    {
        var db = _connectionMultiplexer.GetDatabase();
        var test = db.HashGet(key, "data");
        return test.HasValue ? JsonSerializer.Deserialize<T>(test.ToString()) : null;
    }
    
    public void Dispose()
    {
        _redisContainer?.StopAsync().GetAwaiter().GetResult();
    }
}