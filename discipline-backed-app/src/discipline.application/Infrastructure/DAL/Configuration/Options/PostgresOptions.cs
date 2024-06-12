namespace discipline.application.Infrastructure.DAL.Configuration.Options;

internal sealed record PostgresOptions
{
    internal string ConnectionString { get; set; }
    internal bool WithMigration { get; set; }
}