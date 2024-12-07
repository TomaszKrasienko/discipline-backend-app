using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.domain.ValueObjects;
using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class Activity : Entity<ActivityId>
{
    private List<Stage>? _stages;
    public Details Details { get; private set; } 
    public IsChecked IsChecked { get; private set; }
    public ActivityRuleId? ParentActivityRuleId { get; private set; }
    public IReadOnlyList<Stage>? Stages => _stages;
    
    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Activity(ActivityId id, Details details, IsChecked isChecked, 
        ActivityRuleId? parentActivityRuleId, List<Stage>? stages) : base(id)    
    {
        Details = details;
        IsChecked = isChecked;
        ParentActivityRuleId = parentActivityRuleId;
        _stages = stages;
    }

    internal static Activity Create(ActivityId activityId, ActivityDetailsSpecification details,
        ActivityRuleId? parentActivityRuleId, List<StageSpecification>? stages)
    {
        var activityDetails = Details.Create(details.Title, details.Note);
        var activity = new Activity(activityId, activityDetails, true, 
            parentActivityRuleId, null);
        return activity;
    }
}