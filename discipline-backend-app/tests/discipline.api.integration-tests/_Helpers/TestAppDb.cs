using discipline.application.Infrastructure.DAL.Configuration.Options;
using discipline.application.Infrastructure.DAL.Connection;
using discipline.application.Infrastructure.DAL.Documents;
using MongoDB.Driver;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestAppDb : IDisposable
{
    private const string SectionName = "Mongo";
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollectionNameConvention _mongoCollectionNameConvention;

    public TestAppDb()
    {
        var mongoOptions = new OptionsProvider().Get<MongoOptions>(SectionName);
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(mongoOptions.Database);
        _mongoCollectionNameConvention = new TestsMongoCollectionNameConvention();
    }

    internal IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : IDocument
        => _mongoDatabase.GetCollection<TDocument>(_mongoCollectionNameConvention.GetCollectionName<TDocument>());
    
    public void Dispose()
    {
        var options = new OptionsProvider().Get<MongoOptions>(SectionName);
        _mongoClient.DropDatabase(options.Database);
    }
}