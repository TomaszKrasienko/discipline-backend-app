using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.Repositories;

public interface IReadActivityRuleRepository
{
    Task<bool> ExistsAsync(string title, UserId userId, CancellationToken cancellationToken = default);
    Task<ActivityRule?> GetByIdAsync(ActivityRuleId id, UserId userId, CancellationToken cancellationToken = default);
}