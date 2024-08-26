using discipline.application.Configuration;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Connection.Configuration;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.Users.Repositories;
using discipline.domain.UsersCalendars.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            .AddScoped<IUserRepository, MongoUserRepository>()
            .AddScoped<ISubscriptionRepository, MongoSubscriptionRepository>();

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