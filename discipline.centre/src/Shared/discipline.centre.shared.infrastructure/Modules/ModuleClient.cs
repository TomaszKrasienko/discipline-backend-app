using System.Diagnostics;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.shared.infrastructure.Logging.Configuration.Options;
using discipline.centre.shared.infrastructure.Modules.Abstractions;
using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Modules;

internal sealed class ModuleClient(
    IModuleRegistry moduleRegistry,
    IModuleTypesTranslator moduleTypesTranslator,
    IOptions<OpenTelemetryOptions> openTelemetryOptions)
    : IModuleClient
{
    private readonly ActivitySource _activitySource = new(openTelemetryOptions.Value.InternalSourceName);

    public async Task<TResult?> SendAsync<TResult>(string path, object request) where TResult : class
    {
        var methodName = GetMethodName(path);
        
        using var activity = _activitySource.StartActivity($"{methodName} {path}", ActivityKind.Client);        
        activity?.SetTag("custom.request.path", path);

        try
        {
            var moduleRequestRegistration = moduleRegistry.GetRequestRegistration(path);

            if (moduleRequestRegistration is null)
            {
                throw new InvalidOperationException($"Module for path:{path} not found");
            }

            var receivedRequest = moduleTypesTranslator.Translate(request, moduleRequestRegistration.RequestType);
            var result = await moduleRequestRegistration.Action(receivedRequest);
            activity?.SetTag("custom.client.result", "Success");
            return result is null ? null : moduleTypesTranslator.Translate<TResult>(result);
        }
        catch (Exception ex)
        {
            activity?.SetTag("error", true);
            activity?.SetTag("error.message", ex.Message);
            activity?.SetTag("error.stacktrace", ex.StackTrace);
            throw; 
        }
    }
    
    private static string GetMethodName(string path)
        => path.Split('/').AsEnumerable().Last().ToUpper();
}