using discipline.domain.Users.Entities;
using discipline.domain.Users.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace discipline.application.Infrastructure.DAL;

internal sealed class DbInitializer(
    IServiceProvider serviceProvider,
    ILogger<DbInitializer> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting initialization");
        var subscriptions = new List<Subscription>()
        {
            Subscription.Create(Guid.NewGuid(), "Free", 0, 0, ["Daily habits", "Activity rules"]),
            Subscription.Create(Guid.NewGuid(), "Premium", 10, 100, ["Daily habits", "Activity rules", "Calendar", "Chat"])
        };
        
        using var scope = serviceProvider.CreateScope();
        var subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        var isSubscriptionExists = await subscriptionRepository.AnyAsync(cancellationToken);
        if (!isSubscriptionExists)
        {
            foreach (var subscription in subscriptions)
            {
                await subscriptionRepository.AddAsync(subscription, cancellationToken);
            }
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}