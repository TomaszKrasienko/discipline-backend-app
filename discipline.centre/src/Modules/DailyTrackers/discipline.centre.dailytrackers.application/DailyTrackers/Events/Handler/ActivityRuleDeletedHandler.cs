using discipline.centre.shared.abstractions.Events;
using Microsoft.Extensions.Logging;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Events.Handler;

internal sealed class ActivityRuleDeletedHandler(ILogger<ActivityRuleDeleted> logger) : IEventHandler<ActivityRuleDeleted>
{
    public Task HandleAsync(ActivityRuleDeleted @event)
    {
        logger.LogInformation("ActivityRuleDeleted event handler triggered");
        return Task.CompletedTask;
    }
}