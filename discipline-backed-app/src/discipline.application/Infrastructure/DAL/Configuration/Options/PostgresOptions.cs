namespace discipline.application.Infrastructure.DAL.Configuration.Options;

internal sealed record PostgresOptions
{
    public string ConnectionString { get; set; }
    public bool WithMigration { get; set; }
}