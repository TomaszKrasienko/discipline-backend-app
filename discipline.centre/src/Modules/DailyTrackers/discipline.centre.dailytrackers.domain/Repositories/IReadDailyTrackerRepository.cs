using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain.Repositories;

public interface IReadDailyTrackerRepository
{
    Task<bool> DoesActivityWithActivityRuleExistAsync(ActivityRuleId activityRuleId, UserId userId, DateOnly day,
        CancellationToken cancellationToken); 
}