using discipline.application.Behaviours.Configuration;
using discipline.application.Domain.Users.Services.Configuration;
using discipline.application.Features;
using discipline.application.Infrastructure.DAL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Configuration;

public static class Extensions
{
    private const string CorsName = "discipline-cors";
    
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        => services
            .SetDomain()
            .AddDal(configuration)
            .AddFeatures(configuration)
            .AddDisciplineCors()
            .AddBehaviours()
            .AddSwaggerGen();

    private static IServiceCollection AddDisciplineCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsName, policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyHeader();
            });
        });
        return services;
    }

    public static WebApplication UseApplication(this WebApplication app)
        => app
            .UseUiDocumentation()
            .UseDisciplineCors()
            .UseBehaviours()
            .MapFeatures();

    private static WebApplication UseUiDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    private static WebApplication UseDisciplineCors(this WebApplication app)
    {
        app.UseCors(CorsName);
        return app;
    }

    internal static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        T t = new T();
        configuration.Bind(sectionName, t);
        return t;
    }
}