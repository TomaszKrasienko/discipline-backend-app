namespace discipline.application.Infrastructure.DAL.Configuration.Options;

internal sealed record MongoOptions
{
    public string ConnectionString { get; init; }
    public string Database { get; init; }
    public bool Initialize { get; init; }
}