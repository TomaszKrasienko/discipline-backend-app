using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions;

internal sealed class SubscriptionInitializer(
    IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        List<CreateSubscriptionCommand> commands =
        [
            new CreateSubscriptionCommand(SubscriptionId.New(),
                "Free", 0, 0, ["Daily habits", "Activity rules"]),

            new CreateSubscriptionCommand(SubscriptionId.New(),
                "Premium", 10, 100, ["Daily habits", "Activity rules", "Calendar", "Chat"])
        ];

        using var scope = serviceProvider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<ICqrsDispatcher>();
        foreach (var command in commands)
        {
            await dispatcher.HandleAsync(command, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}