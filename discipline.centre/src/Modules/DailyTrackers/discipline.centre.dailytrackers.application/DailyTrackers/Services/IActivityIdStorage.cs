using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Services;

/// <summary>
/// Interface for managing the storage of activity identifier 
/// </summary>
public interface IActivityIdStorage
{
    /// <summary>
    /// Sets <see cref="ActivityId"/> in storage
    /// </summary>
    /// <param name="activityId">Instance of <see cref="ActivityId"/> to be stored</param>
    void Set(ActivityId activityId);
    
    /// <summary>
    /// Get stored <see cref="ActivityId"/>
    /// </summary>
    /// <returns>The instance of <see cref="ActivityId"/> if exists, otherwise <c>null</c></returns>
    ActivityId? Get();
}