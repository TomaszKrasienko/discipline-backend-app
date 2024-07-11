using discipline.application.Infrastructure.DAL.Documents;

namespace discipline.application.Infrastructure.DAL.Connection;

internal interface IMongoCollectionNameConvention
{
    string GetCollectionName<T>() where T : IDocument;
}