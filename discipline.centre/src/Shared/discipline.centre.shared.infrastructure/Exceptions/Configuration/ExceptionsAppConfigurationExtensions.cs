// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class ExceptionsAppConfigurationExtensions
{
    internal static WebApplication UseExceptionsHandling(this WebApplication app)
    {
        app.UseExceptionHandler();
        return app;
    }
}