namespace discipline.centre.shared.infrastructure.DAL.Configuration;

internal sealed record MongoDbOptions
{
    public string? ConnectionString { get; init; }
}