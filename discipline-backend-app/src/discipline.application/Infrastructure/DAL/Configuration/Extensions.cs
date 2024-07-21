using System.Collections.Frozen;
using discipline.application.Configuration;
using discipline.application.Domain.ActivityRules.Repositories;
using discipline.application.Domain.DailyProductivities.Repositories;
using discipline.application.Domain.Users.Repositories;
using discipline.application.Domain.UsersCalendars.Repositories;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Connection.Configuration;
using discipline.application.Infrastructure.DAL.Documents;
using discipline.application.Infrastructure.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Configuration;

internal static class Extensions
{
    private const string SectionName = "Mongo";
    
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddServices()
            .AddOptions(configuration)
            .AddMongoConnection()
            .AddInitializer(configuration);

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MongoOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IDailyProductivityRepository, MongoDailyProductivityRepository>()
            .AddScoped<IUserCalendarRepository, MongoUserCalendarRepository>()
            .AddScoped<IUserRepository, MongoUserRepository>();

    private static IServiceCollection AddInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<MongoOptions>(SectionName);
        if (options.Initialize)
        {
            services.AddHostedService<DbInitializer>();
        }

        return services;
    }
}