using discipline.application.Infrastructure.DAL.Configuration.Options;
using MongoDB.Driver;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestAppDb : IDisposable
{
    private const string SectionName = "Mongo";
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    internal IMongoDatabase MongoDatabase { get; set; }

    public TestAppDb()
    {
        var mongoOptions = new OptionsProvider().Get<MongoOptions>(SectionName);
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(mongoOptions.Database);
    }

    internal IMongoCollection<T> GetCollection<T>(string collectionName)
        => _mongoDatabase.GetCollection<T>(collectionName);
    
    public void Dispose()
    {
        var options = new OptionsProvider().Get<MongoOptions>(SectionName);
        _mongoClient.DropDatabase(options.Database);
    }
}