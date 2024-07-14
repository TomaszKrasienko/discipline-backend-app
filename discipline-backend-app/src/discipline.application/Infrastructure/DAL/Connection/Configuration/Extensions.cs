using discipline.application.Infrastructure.DAL.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Connection.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddMongoConnection(this IServiceCollection services)
        => services
            .AddConnection()
            .AddServices();

    
    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<IMongoCollectionNameConvention, MongoCollectionNameConvention>()
            .AddTransient<IDisciplineMongoCollection, DisciplineMongoCollection>();
    
    private static IServiceCollection AddConnection(this IServiceCollection services)
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
        
        services.AddTransient<IDisciplineMongoCollection>(sp =>
        {
            var mongoDatabase = sp.GetRequiredService<IMongoDatabase>();
            var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
            return new DisciplineMongoCollection(
                mongoDatabase,
                mongoCollectionNameConvention);
        });
        return services;
    }
}