using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.DAL.Configuration;
using MongoDB.Driver;

namespace discipline.centre.e2e_tests.shared;

public sealed class TestAppDb : IDisposable
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollectionNameConvention _mongoCollectionNameConvention;
    private readonly string _databaseName;
           
    public TestAppDb(string databaseName)
    {
        _databaseName = databaseName;
        var mongoOptions = new OptionsProvider().Get<MongoDbOptions>();
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(databaseName);
        _mongoCollectionNameConvention = new TestsMongoCollectionNameConvention();
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : IDocument
        => _mongoDatabase
            .GetCollection<TDocument>(
                _mongoCollectionNameConvention.GetCollectionName<TDocument>());
    
    public void Dispose()
    {
        _mongoClient.DropDatabase(_databaseName);
    }
}