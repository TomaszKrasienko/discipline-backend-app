using discipline.application.Infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.application.Infrastructure.DAL;

public interface IDisciplineMongoCollection
{
    IMongoCollection<T> GetCollection<T>() where T : IDocument;
}