using discipline.domain.ActivityRules.Repositories;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.Users.Repositories;
using discipline.domain.UsersCalendars.Repositories;
using discipline.infrastructure.DAL;
using discipline.infrastructure.DAL.Configuration.Options;
using discipline.infrastructure.DAL.Connection.Configuration;
using discipline.infrastructure.DAL.Repositories;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddServices()
            .AddOptions(configuration)
            .AddMongoConnection()
            .AddInitializer();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IWriteUserRepository, MongoUserRepository>()
            .AddScoped<IReadUserRepository, MongoUserRepository>()
            .AddScoped<IActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IDailyProductivityRepository, MongoDailyProductivityRepository>()
            .AddScoped<IUserCalendarRepository, MongoUserCalendarRepository>()
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