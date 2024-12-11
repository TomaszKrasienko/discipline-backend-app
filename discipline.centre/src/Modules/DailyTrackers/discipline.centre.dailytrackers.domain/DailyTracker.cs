using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
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

    public static DailyTracker Create(DailyTrackerId id, DateOnly day, UserId userId, ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        var dailyTracker = new DailyTracker(id, day, userId);
        dailyTracker.AddActivity(details, parentActivityRuleId, stages);
        return dailyTracker;
    }

    public Activity AddActivity(ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        if (_activities.Exists(x => x.Details.Title == details.Title))
        {
            throw new DomainException("DailyTracker.Activity.Title.AlreadyExists",
                $"Activity with title '{details.Title}' already exists.");
        }
        
        var activity = Activity.Create(ActivityId.New(), details, parentActivityRuleId, stages);
        _activities.Add(activity);
        return activity;
    }
}