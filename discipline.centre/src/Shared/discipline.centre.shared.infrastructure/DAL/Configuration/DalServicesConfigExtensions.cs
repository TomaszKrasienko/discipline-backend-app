using discipline.centre.shared.infrastructure.DAL.Collections;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.DAL.Configuration;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DalServicesConfigExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration)
            .AddConnection()
            .AddCollections();

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<MongoDbOptions, MongoDbOptionsValidator>(configuration);
    
    private static IServiceCollection AddConnection(this IServiceCollection services)
    {
        var mongoOptions = services.GetOptions<MongoDbOptions>();
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoOptions.ConnectionString));
        return services;
    }

    private static IServiceCollection AddCollections(this IServiceCollection services)
        => services
            .AddSingleton<IMongoCollectionNameConvention, MongoCollectionNameConvention>();

    public static IServiceCollection AddMongoContext(this IServiceCollection services, string name)
        => services.AddTransient<IMongoCollectionContext>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase($"{name}");
            var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
            return new MongoCollectionContext(
                database,
                mongoCollectionNameConvention);
        });


}