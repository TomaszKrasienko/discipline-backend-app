using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Infrastructure.DAL.Connection.Configuration;

internal static class Extensions
{
    internal static IServiceCollection AddMongoConnection(this IServiceCollection services)
        => services
            .AddSingleton<IMongoCollectionNameConvention, MongoCollectionNameConvention>()
            .AddTransient<IDisciplineMongoCollection, DisciplineMongoCollection>();
}