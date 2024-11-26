namespace discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;

public interface IMongoCollectionNameConvention
{
    string GetCollectionName<T>() where T : IDocument;
}