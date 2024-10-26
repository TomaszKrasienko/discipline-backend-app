using discipline.infrastructure.DAL.Connection;
using discipline.infrastructure.DAL.Documents;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestsMongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => $"{typeof(T).Name}s_tests";
}