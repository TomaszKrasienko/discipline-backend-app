using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

namespace discipline.centre.shared.infrastructure.DAL.Collections;

internal sealed class MongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => $"{typeof(T).Name}s";
}