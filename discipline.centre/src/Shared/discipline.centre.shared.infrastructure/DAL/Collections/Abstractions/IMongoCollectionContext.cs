using MongoDB.Driver;

namespace discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

public interface IMongoCollectionContext
{
    IMongoCollection<T> GetCollection<T>() where T : IDocument;
}