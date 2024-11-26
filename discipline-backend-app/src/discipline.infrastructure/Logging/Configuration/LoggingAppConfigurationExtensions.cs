using Serilog;
using Serilog.Events;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class LoggingAppConfigurationExtensions
{
    internal static WebApplicationBuilder UseLogging(this WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}");
        });
        return app;
    }
}