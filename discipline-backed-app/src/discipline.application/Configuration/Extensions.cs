using discipline.application.Behaviours.Configuration;
using discipline.application.Features.Configuration;
using discipline.application.Infrastructure.DAL.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Configuration;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDal(configuration)
            .AddCqrs()
            .AddBehaviours()
            .AddSwaggerGen();

    public static WebApplication UseApplication(this WebApplication app)
        => app
            .UseUiDocumentation()
            .UseBehaviours()
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