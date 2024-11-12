namespace discipline.centre.activityrules.domain.Repositories;

public interface IWriteActivityRuleRepository
{
    Task AddAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task UpdateAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
    Task DeleteAsync(ActivityRule activityRule, CancellationToken cancellationToken = default);
}