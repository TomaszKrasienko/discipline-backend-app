using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.Extensions.Logging;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Events.Handler;

internal sealed class ActivityRuleDeletedHandler(IReadWriteDailyTrackerRepository readWriteDailyTrackerRepository) : IEventHandler<ActivityRuleDeleted>
{
    public async Task HandleAsync(ActivityRuleDeleted @event, CancellationToken cancellationToken)
    {
        var activityRuleId = new ActivityRuleId(@event.ActivityRuleId);
        var userId = new UserId(@event.UserId);

        var dailyTrackers = await readWriteDailyTrackerRepository
            .GetDailyTrackersByParentActivityRuleId(activityRuleId, userId, cancellationToken);

        foreach (var dailyTracker in dailyTrackers)
        {
            dailyTracker.ClearParentActivityRuleIdIs(activityRuleId);
        }

        await readWriteDailyTrackerRepository.UpdateRangeAsync(dailyTrackers, cancellationToken);
    }
}