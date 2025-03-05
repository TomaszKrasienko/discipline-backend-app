using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.DAL.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace discipline.centre.integration_tests.shared;

public sealed class TestAppDb : IDisposable
{
    private MongoDbContainer? _mongoDbContainer;
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollectionNameConvention _mongoCollectionNameConvention;
    private readonly string _databaseName;
           
    public TestAppDb(string databaseName)
    {
        _databaseName = databaseName;
        CreateContainer();
        var mongoOptions = GetOptions().Value;
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(databaseName);
        _mongoCollectionNameConvention = new TestsMongoCollectionNameConvention();
    }

    private void CreateContainer()
    {
        _mongoDbContainer = new MongoDbBuilder()
            .Build();
        _mongoDbContainer.StartAsync().GetAwaiter().GetResult();
    }

    internal IMongoClient GetMongoClient()
        => _mongoClient;

    private IOptions<MongoDbOptions> GetOptions()
    {
        if (_mongoDbContainer is null)
        {
            throw new ArgumentException("Mongo container can not be null");
        }
        return Options.Create(new MongoDbOptions()
        {
            ConnectionString = _mongoDbContainer.GetConnectionString()
        });
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : IDocument
        => _mongoDatabase
            .GetCollection<TDocument>(
                _mongoCollectionNameConvention.GetCollectionName<TDocument>());

    public IMongoCollection<TDocument> GetCollection<TDocument>(string databaseName) where TDocument : IDocument
    {
        var mongoDatabase = _mongoClient.GetDatabase(databaseName);
        return mongoDatabase.GetCollection<TDocument>(
                _mongoCollectionNameConvention.GetCollectionName<TDocument>());
    }
    
    public void Dispose()
    {
        _mongoClient.DropDatabase(_databaseName);
        _mongoDbContainer?.StopAsync().GetAwaiter().GetResult();
    }
}