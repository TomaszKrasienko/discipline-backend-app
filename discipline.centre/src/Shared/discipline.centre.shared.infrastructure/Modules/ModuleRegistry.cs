using discipline.centre.shared.infrastructure.Modules.Abstractions;
using discipline.centre.shared.infrastructure.Modules.Types;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleRegistry : IModuleRegistry
{
    private readonly Dictionary<string, ModuleRequestRegistration> _requestRegistrations = [];
    private readonly List<ModuleBroadcastRegistration> _broadcastRegistrations = [];
    
    public ModuleRequestRegistration? GetRequestRegistration(string path)
        => _requestRegistrations.TryGetValue(path, out var registration) ? registration : null;

    public void AddRequestRegistration(string path, Type requestType, Type? responseType, Func<object, Task<object?>> action)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException("Path could not be null or empty.");
        }

        var result = _requestRegistrations.TryAdd(path, 
            new ModuleRequestRegistration(requestType, requestType, action));

        if (!result)
        {
            throw new InvalidOperationException($"Failed to add Module Registration for {path}.");
        }
    }

    public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key)
        => _broadcastRegistrations.Where(x => x.Key == key);

    public void AddBroadcastingRegistration(Type requestType, Func<object, Task> action)
    {
        if (string.IsNullOrWhiteSpace(requestType.Namespace))
        {
            throw new InvalidOperationException("Missing namespace");
        }

        var registration = new ModuleBroadcastRegistration(requestType, action);
        _broadcastRegistrations.Add(registration);
    }
}