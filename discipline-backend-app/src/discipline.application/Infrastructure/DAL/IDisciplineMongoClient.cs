using discipline.application.Infrastructure.DAL.Documents;

namespace discipline.application.Infrastructure.DAL;

public interface IDisciplineMongoCollection<T> where T : IDocument
{
    IDisciplineMongoCollection<T> GetCollection();
}