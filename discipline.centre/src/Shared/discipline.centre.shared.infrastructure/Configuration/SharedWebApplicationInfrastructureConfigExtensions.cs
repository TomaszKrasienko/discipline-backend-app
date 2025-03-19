using discipline.centre.shared.infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class SharedWebApplicationInfrastructureConfigExtensions
{
    public static IApplicationBuilder UseInfrastructure(this WebApplication app)
        => app
            .UseUiDocumentation()
            .UseExceptionsHandling()
            .UseCorsPolicy()
            .UseAuth()
            .UseLogging();

    private static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        app.UseCors("CorsPolicy");
        return app;
    } 
    
    private static IApplicationBuilder UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
    
    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        var appOptions = app.Services.GetRequiredService<IOptions<AppOptions>>().Value;
        app.UseSwagger();
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = "redoc";
            reDoc.SpecUrl("/swagger/v1/swagger.json");
            reDoc.DocumentTitle = appOptions.Name;
        });
        return app;
    }
}