using discipline.application.Configuration;
using discipline.application.Domain.Repositories;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Repositories;
using discipline.application.Infrastructure.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Configuration;

internal static class Extensions
{
    private const string SectionName = "Postgres";
    private const string MongoSectionName = "Mongo";
    
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgreSqlDbContext(configuration)
            .AddServices()
            .AddInitializer(configuration)
            .AddOptions(configuration)
            .AddMongoConnection();

    private static IServiceCollection AddPostgreSqlDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<PostgresOptions>(SectionName);
        services.AddDbContext<DisciplineDbContext>(x => x.UseNpgsql(options.ConnectionString));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<MongoOptions>(configuration.GetSection(MongoSectionName));
    
    private static IServiceCollection AddMongoConnection(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            return new MongoClient(options.ConnectionString);
        });
        services.AddTransient(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.Database);
        });
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, PostgresUnitOfWork>()
            .AddScoped<IActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IDailyProductivityRepository, MongoDailyProductivityRepository>();

    private static IServiceCollection AddInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<PostgresOptions>(SectionName);
        if (options.WithMigration)
        {
            services.AddHostedService<DatabaseInitializer>();
        }

        return services;
    }
}