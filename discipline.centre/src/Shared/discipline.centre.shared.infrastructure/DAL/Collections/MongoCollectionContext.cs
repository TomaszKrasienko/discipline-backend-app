using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.shared.infrastructure.DAL.Collections;

internal sealed class MongoCollectionContext(
    IMongoDatabase mongoDatabase,
    IMongoCollectionNameConvention mongoCollectionNameConvention) : IMongoCollectionContext
{
    public IMongoCollection<T> GetCollection<T>() where T : IDocument
    {
        var collectionName = mongoCollectionNameConvention.GetCollectionName<T>();
        return mongoDatabase.GetCollection<T>(collectionName);
    }
}