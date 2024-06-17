using discipline.application.Domain.Entities;
using discipline.application.Domain.ValueObjects.ActivityRules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DatabaseInitializer(
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DisciplineDbContext>();
        
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (!await dbContext.ActivityRules.AnyAsync(cancellationToken))
        {
            var activityRules = new List<ActivityRule>
            {
                ActivityRule.Create(Guid.NewGuid(), "Morning stretching", Mode.EveryDayMode()),
                ActivityRule.Create(Guid.NewGuid(), "Browsing mails", Mode.EveryDayMode()),
                ActivityRule.Create(Guid.NewGuid(), "Writing thoughts", Mode.CustomMode(), [0]),
                ActivityRule.Create(Guid.NewGuid(), "Cleaning", Mode.CustomMode(), [3, 6]),
                ActivityRule.Create(Guid.NewGuid(), "Coffee break", Mode.EveryDayMode()),
                ActivityRule.Create(Guid.NewGuid(), "Savings", Mode.FirstDayOfMonth())
            };
            await dbContext.ActivityRules.AddRangeAsync(activityRules, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}