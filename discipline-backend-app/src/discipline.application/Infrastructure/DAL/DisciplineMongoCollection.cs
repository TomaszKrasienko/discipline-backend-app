using System.Collections.Frozen;
using discipline.application.Infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DisciplineMongoCollection(
    IMongoDatabase mongoDatabase,
    FrozenDictionary<Type, string> collectionsDictionary) : IDisciplineMongoCollection
{
    public IMongoCollection<T> GetCollection<T>() where T : IDocument
    {
        if (!collectionsDictionary.TryGetValue(typeof(T), out var name))
        {
            throw new Exception("Can not find registered documents");
        }

        var collectionName = $"{name}s";
        return mongoDatabase.GetCollection<T>(collectionName);
    }
}