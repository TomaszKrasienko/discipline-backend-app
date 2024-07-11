using System.Collections.Frozen;
using discipline.application.Configuration;
using discipline.application.Domain.ActivityRules.Repositories;
using discipline.application.Domain.DailyProductivities.Repositories;
using discipline.application.Infrastructure.DAL.Configuration.Options;
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
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var documentsType = assemblies.SelectMany(x => x.GetTypes())
            .Where(x => typeof(IDocument).IsAssignableFrom(x) && !x.IsInterface);
        var collectionDictionary = documentsType
            .ToFrozenDictionary(x => x, x => x.Name);
        
        services.AddTransient<IDisciplineMongoCollection>(sp =>
        {
            var mongoDatabase = sp.GetRequiredService<IMongoDatabase>();
            return new DisciplineMongoCollection(
                mongoDatabase,
                collectionDictionary);
        });
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IActivityRuleRepository, MongoActivityRuleRepository>()
            .AddScoped<IDailyProductivityRepository, MongoDailyProductivityRepository>();

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