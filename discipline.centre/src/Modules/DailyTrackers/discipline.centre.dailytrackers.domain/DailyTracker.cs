using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects.DailyTrackers;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class DailyTracker : AggregateRoot<DailyTrackerId, Ulid>
{
    private readonly List<Activity> _activities = [];
    public Day Day { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<Activity> Activities => _activities;

    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public DailyTracker(DailyTrackerId id, Day day, UserId userId, List<Activity> activities) : this(id,
        day, userId)
        => _activities = activities;

    private DailyTracker(DailyTrackerId id, Day day, UserId userId) : base(id)
    {
        Day = day;
        UserId = userId;
    }

    public static DailyTracker Create(DailyTrackerId id, DateOnly day, UserId userId, ActivityId activityId,
        ActivityDetailsSpecification details, ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        var dailyTracker = new DailyTracker(id, day, userId);
        dailyTracker.AddActivity(activityId, details, parentActivityRuleId, stages);
        return dailyTracker;
    }

    public Activity AddActivity(ActivityId activityId, ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        if (_activities.Exists(x => x.Details.Title == details.Title))
        {
            throw new DomainException("DailyTracker.Activity.Title.AlreadyExists",
                $"Activity with title '{details.Title}' already exists.");
        }
        
        var activity = Activity.Create(activityId, details, parentActivityRuleId, stages);
        _activities.Add(activity);
        return activity;
    }

    public void MarkActivityAsChecked(ActivityId activityId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);

        if (activity is null)
        {
            throw new DomainException("DailyTracker.Activity.NotExists",
                $"Activity with ID: '{activityId}' does not exist.");
        }
        
        activity.MarkAsChecked();
    }
    
    public void MarkActivityStageAsChecked(ActivityId activityId, StageId stageId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);
        
        if (activity is null)
        {
            throw new DomainException("DailyTracker.Activity.NotFound",
                $"Activity with id {activityId} not found.");    
        }
        
        activity.MarkStageAsChecked(stageId);
    }

    public void DeleteActivityStage(ActivityId activityId, StageId stageId)
    {
        var activity = _activities.SingleOrDefault(x => x.Id == activityId);

        if (activity is null)
        {
            return;
        }
        
        activity.DeleteStage(stageId);
    }
}