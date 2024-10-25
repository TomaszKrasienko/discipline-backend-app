using discipline.domain.ActivityRules.Repositories;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.Users.Repositories;
using discipline.domain.UsersCalendars.Repositories;
using discipline.infrastructure.Configuration;
using discipline.infrastructure.DAL.Configuration.Options;
using discipline.infrastructure.DAL.Connection.Configuration;
using discipline.infrastructure.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.infrastructure.DAL.Configuration;

internal static class Extensions
{
    private const string SectionName = "Mongo";

    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddServices()
            .AddOptions(configuration)
            .AddMongoConnection()
            .AddInitializer();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MongoOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IDailyProductivityRepository, MongoDailyProductivityRepository>()
            .AddScoped<IUserCalendarRepository, MongoUserCalendarRepository>()
            .AddScoped<IUserRepository, MongoUserRepository>()
            .AddScoped<ISubscriptionRepository, MongoSubscriptionRepository>();

    private static IServiceCollection AddInitializer(this IServiceCollection services)
    {
        var options = services.GetOptions<MongoOptions>().Value;
        if (options.Initialize)
        {
            services.AddHostedService<DbInitializer>();
        }

        return services;
    }
}