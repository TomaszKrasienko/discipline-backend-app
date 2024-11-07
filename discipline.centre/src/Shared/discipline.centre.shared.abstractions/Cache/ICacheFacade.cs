namespace discipline.centre.shared.abstractions.Cache;

public interface ICacheFacade
{
    Task Add<T>(string key, T value) where T : class;
}