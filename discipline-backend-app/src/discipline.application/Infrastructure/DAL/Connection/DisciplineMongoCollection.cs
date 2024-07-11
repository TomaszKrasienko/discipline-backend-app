using discipline.application.Infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL.Connection;

internal sealed class DisciplineMongoCollection(
    IMongoDatabase mongoDatabase,
    IMongoCollectionNameConvention mongoCollectionNameConvention) : IDisciplineMongoCollection
{
    public IMongoCollection<T> GetCollection<T>() where T : IDocument
    {
        var collectionName = mongoCollectionNameConvention.GetCollectionName<T>();
        return mongoDatabase.GetCollection<T>(collectionName);
    }
}