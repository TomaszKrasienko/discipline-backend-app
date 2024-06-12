using discipline.application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DisciplineDbContext(DbContextOptions<DisciplineDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<ActivityRule> ActivityRules { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}