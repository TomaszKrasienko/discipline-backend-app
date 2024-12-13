using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Modules.Abstractions;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleClient(
    IModuleRegistry moduleRegistry,
    IModuleTypesTranslator moduleTypesTranslator) : IModuleClient
{
    public async Task<TResult?> SendAsync<TResult>(string path, object request) where TResult : class
    {
        var moduleRequestRegistration = moduleRegistry.GetRequestRegistration(path);

        if (moduleRequestRegistration is null)
        {
            throw new InvalidOperationException($"Module for path:{path} not found");
        }
        
        var receivedRequest = moduleTypesTranslator.Translate(request, moduleRequestRegistration.RequestType);
        var result = await moduleRequestRegistration.Action(receivedRequest);
        return result is null ? null : moduleTypesTranslator.Translate<TResult>(result);
    }
}