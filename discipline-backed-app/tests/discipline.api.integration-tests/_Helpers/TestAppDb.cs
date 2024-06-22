using discipline.application.Infrastructure.DAL;
using discipline.application.Infrastructure.DAL.Configuration.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace discipline.api.integration_tests._Helpers;

internal sealed class TestAppDb : IDisposable
{
    private const string SectionName = "Postgres";
    private const string MongoSectionName = "Mongo";
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    
    internal DisciplineDbContext DisciplineDbContext { get; set; }
    internal IMongoDatabase MongoDatabase { get; set; }

    public TestAppDb()
    {
        var options = new OptionsProvider().Get<PostgresOptions>(SectionName);
        var contextOptions = new DbContextOptionsBuilder<DisciplineDbContext>()
            .UseNpgsql(options.ConnectionString)
            .EnableSensitiveDataLogging()
            .Options;
        DisciplineDbContext = new DisciplineDbContext(contextOptions);
        
        var mongoOptions = new OptionsProvider().Get<MongoOptions>(MongoSectionName);
        _mongoClient = new MongoClient(mongoOptions.ConnectionString);
        _mongoDatabase = _mongoClient.GetDatabase(mongoOptions.Database);
    }

    internal IMongoCollection<T> GetCollection<T>(string collectionName)
        => _mongoDatabase.GetCollection<T>(collectionName);
    
    public void Dispose()
    {
        DisciplineDbContext.Database.EnsureDeleted();
        
        var options = new OptionsProvider().Get<MongoOptions>(MongoSectionName);
        _mongoClient.DropDatabase(options.Database);
    }
}