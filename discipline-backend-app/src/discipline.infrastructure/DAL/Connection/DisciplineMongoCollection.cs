using discipline.infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Connection;

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