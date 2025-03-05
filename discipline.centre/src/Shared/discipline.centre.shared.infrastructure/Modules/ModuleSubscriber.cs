using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Modules.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleSubscriber(
    IModuleRegistry moduleRegistry,
    IServiceProvider serviceProvider) : IModuleSubscriber
{
    public IModuleSubscriber MapModuleRequest<TRequest, TResponse>(string path, 
        Func<TRequest, IServiceProvider, Task<TResponse?>> action)  where TRequest : class where TResponse : class
    {
        moduleRegistry.AddRequestRegistration(path, typeof(TRequest), typeof(TResponse),
            async request => await action((TRequest)request, serviceProvider));
        
        return this;
    }
}