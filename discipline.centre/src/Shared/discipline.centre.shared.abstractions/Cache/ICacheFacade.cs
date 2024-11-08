namespace discipline.centre.shared.abstractions.Cache;

public interface ICacheFacade
{
    Task AddAsync<T>(string key, T value, TimeSpan expiration) where T : class;
}