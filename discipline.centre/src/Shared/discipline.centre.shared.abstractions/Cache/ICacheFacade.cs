namespace discipline.centre.shared.abstractions.Cache;

public interface ICacheFacade
{
    Task AddOrUpdateAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken) where T : class;
    Task AddOrUpdateAsync<T>(string key, T value, CancellationToken cancellationToken) where T : class;
    Task DeleteAsync(string key, CancellationToken cancellationToken);
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
}