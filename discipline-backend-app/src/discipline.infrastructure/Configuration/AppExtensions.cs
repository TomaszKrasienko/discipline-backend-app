using discipline.infrastructure.Exceptions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

public static class AppExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app)
        => app
            .UseAuth()
            .UseExceptions();
}