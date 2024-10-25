namespace discipline.infrastructure.DAL.Configuration.Options;

internal sealed record MongoOptions
{
    public required string ConnectionString { get; init; }
    public required string Database { get; init; }
    public bool Initialize { get; init; }
}