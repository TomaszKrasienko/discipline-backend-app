using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.CacheDecorators;

internal sealed class CacheDailyTrackerRepositoryDecorator(
    IReadWriteDailyTrackerRepository readWriteDailyTrackerRepository,
    ICacheFacade cacheFacade) : IReadWriteDailyTrackerRepository
{
    public async Task<DailyTracker?> GetDailyTrackerByDayAsync(UserId userId, DateOnly day, CancellationToken cancellationToken)
    {
        var cachedData = await cacheFacade.GetAsync<DailyTrackerDocument>(GetCacheKey(userId, day.ToString()), cancellationToken);

        if (cachedData is not null)
        {
            return cachedData.AsEntity();
        }
        
        var result = await readWriteDailyTrackerRepository.GetDailyTrackerByDayAsync(userId, day, cancellationToken);

        if (result is null)
        {
            return null;
        }

        await AddOrUpdateToCacheAsync(result, cancellationToken);
        return result;

    }

    public async Task<DailyTracker?> GetDailyTrackerByIdAsync(UserId userId, DailyTrackerId id, CancellationToken cancellationToken)
    {
        var cachedData = await cacheFacade.GetAsync<DailyTrackerDocument>(GetCacheKey(userId, id.ToString()), cancellationToken);

        if (cachedData is not null)
        {
            return cachedData.AsEntity();
        }
        
        var result = await readWriteDailyTrackerRepository.GetDailyTrackerByIdAsync(userId, id, cancellationToken);

        if (result is null)
        {
            return null;
        }
            
        await AddOrUpdateToCacheAsync(result, cancellationToken);
            
        return result;
    }

    public Task<List<DailyTracker>> GetDailyTrackersByParentActivityRuleId(UserId userId, ActivityRuleId activityRuleId,
        CancellationToken cancellationToken)
        => readWriteDailyTrackerRepository.GetDailyTrackersByParentActivityRuleId(userId, activityRuleId,
            cancellationToken);

    public async Task AddAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await AddOrUpdateToCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.AddAsync(dailyTracker, cancellationToken);
    }

    public async Task UpdateAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await AddOrUpdateToCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<DailyTracker> dailyTrackers, CancellationToken cancellationToken)
    {
        var trackers = dailyTrackers.ToList();
        
        foreach (var dailyTracker in trackers)
        {
            await RemoveFromCacheAsync(dailyTracker, cancellationToken);
        }
        
        await readWriteDailyTrackerRepository.UpdateRangeAsync(trackers, cancellationToken);
    }

    public async Task DeleteAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await RemoveFromCacheAsync(dailyTracker, cancellationToken);
        await readWriteDailyTrackerRepository.DeleteAsync(dailyTracker, cancellationToken);
    }

    private async Task AddOrUpdateToCacheAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await cacheFacade.AddOrUpdateAsync(GetCacheKey(dailyTracker.UserId, dailyTracker.Day.Value.ToString()), 
            dailyTracker.AsDocument(), cancellationToken);
        await cacheFacade.AddOrUpdateAsync(GetCacheKey(dailyTracker.UserId, dailyTracker.Id.ToString()), 
            dailyTracker.AsDocument(), cancellationToken);
    }

    private async Task RemoveFromCacheAsync(DailyTracker dailyTracker, CancellationToken cancellationToken)
    {
        await cacheFacade.DeleteAsync(GetCacheKey(dailyTracker.UserId, dailyTracker.Day.Value.ToString()),
            cancellationToken);
        await cacheFacade.DeleteAsync(GetCacheKey(dailyTracker.UserId,dailyTracker.Id.ToString()),
            cancellationToken);
    }
    
    internal static string GetCacheKey(UserId userId, string param)
        => $"{userId.ToString()}:{param}";
}