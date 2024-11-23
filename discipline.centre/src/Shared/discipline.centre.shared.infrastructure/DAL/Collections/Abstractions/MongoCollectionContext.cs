using MongoDB.Driver;

namespace discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

public abstract class MongoCollectionContext(
    IMongoClient mongoClient,
    IMongoCollectionNameConvention mongoCollectionNameConvention,
    string moduleName) : IMongoCollectionContext
{
    public IMongoCollection<T> GetCollection<T>() where T : IDocument
    {
        var mongoDatabase = mongoClient.GetDatabase(moduleName);
        var collectionName = mongoCollectionNameConvention.GetCollectionName<T>();
        return mongoDatabase.GetCollection<T>(collectionName);
    }
}