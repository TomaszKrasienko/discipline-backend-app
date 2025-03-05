using discipline.centre.shared.infrastructure.Modules.Types;

namespace discipline.centre.shared.infrastructure.Modules.Abstractions;

internal interface IModuleRegistry
{
    ModuleRequestRegistration? GetRequestRegistration(string path);
    void AddRequestRegistration(string path, Type requestType, Type? responseType, Func<object, Task<object?>> action);
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
    void AddBroadcastingRegistration(Type requestType, Func<object, Task> action);
}