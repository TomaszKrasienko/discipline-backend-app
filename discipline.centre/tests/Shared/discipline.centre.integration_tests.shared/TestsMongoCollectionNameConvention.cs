using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

namespace discipline.centre.integration_tests.shared;

internal sealed class TestsMongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => $"{typeof(T).Name}s_tests";
}