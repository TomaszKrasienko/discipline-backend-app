using discipline.infrastructure.DAL.Documents;

namespace discipline.infrastructure.DAL.Connection;

internal sealed class MongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => $"{typeof(T).Name}s";
}