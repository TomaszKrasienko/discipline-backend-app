using System.Collections.Frozen;
using discipline.application.Infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DisciplineMongoClient<T>(
    IMongoDatabase mongoDatabase,
    FrozenDictionary<Type, string> collectionsDictionary) : IDisciplineMongoCollection<T> where T : IDocument
{
    public IDisciplineMongoCollection<T> GetCollection()
    {
        if (collectionsDictionary.TryGetValue(typeof(T), out var name))
        {
            throw new Exception("Can not find registered documents");
        }

        return null;
    }
}