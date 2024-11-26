using discipline.infrastructure.DAL.Documents;

namespace discipline.infrastructure.DAL.Connection;

internal interface IMongoCollectionNameConvention
{
    string GetCollectionName<T>() where T : IDocument;
}