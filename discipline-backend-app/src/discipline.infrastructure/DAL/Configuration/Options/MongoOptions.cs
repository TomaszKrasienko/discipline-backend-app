namespace discipline.infrastructure.DAL.Configuration.Options;

internal sealed record MongoOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
    public bool Initialize { get; init; }
}