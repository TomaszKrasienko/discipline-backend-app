using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class DailyTracker : AggregateRoot<DailyTrackerId>
{
    private readonly List<Activity> _activities = [];
    public Day Day { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<Activity> Activities => _activities;

    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public DailyTracker(DailyTrackerId id, Day day, UserId userId) : base(id)
    {
        Day = day;
        UserId = userId;
    }

    public static DailyTracker Create(DailyTrackerId id, Day day, UserId userId)
        => new DailyTracker(id, day, userId);
    
    
}