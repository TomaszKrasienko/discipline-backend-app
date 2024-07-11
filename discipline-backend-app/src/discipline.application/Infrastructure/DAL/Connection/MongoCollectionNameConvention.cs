using discipline.application.Infrastructure.DAL.Documents;

namespace discipline.application.Infrastructure.DAL.Connection;

internal sealed class MongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => "{typeof(T).Name}s";
}