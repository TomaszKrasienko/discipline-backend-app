using discipline.infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.infrastructure.DAL.Connection;

public interface IDisciplineMongoCollection
{
    IMongoCollection<T> GetCollection<T>() where T : IDocument;
}