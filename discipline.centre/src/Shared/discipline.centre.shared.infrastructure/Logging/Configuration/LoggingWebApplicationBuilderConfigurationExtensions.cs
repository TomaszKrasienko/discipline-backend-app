using discipline.centre.shared.infrastructure.Logging.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class LoggingWebApplicationBuilderConfigurationExtensions
{
    internal static WebApplicationBuilder UseLogging(this WebApplicationBuilder app)
    {
        var options = app.Services.GetOptions<SeqOptions>();
        
        app.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}")
                .WriteTo.Seq(options.Url);
        });

        return app;
    }
}