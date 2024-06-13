using discipline.application.Behaviours;
using discipline.application.Features.Configuration;
using discipline.application.Infrastructure.DAL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Configuration;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddHandlingException()
            .AddDal(configuration)
            .AddCqrs()
            .AddSwaggerGen();

    public static WebApplication UseApplication(this WebApplication app)
        => app
            .UseUiDocumentation()
            .UseHandlingException()
            .MapFeatures();

    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        T t = new T();
        configuration.Bind(sectionName, t);
        return t;
    }
}