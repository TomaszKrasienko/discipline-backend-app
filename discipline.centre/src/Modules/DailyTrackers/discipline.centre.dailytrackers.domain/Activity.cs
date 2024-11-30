using discipline.centre.dailytrackers.domain.ValueObjects.Activities;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.domain;

public sealed class Activity : Entity<ActivityId>
{
    public Details Details { get; private set; } 
    public IsChecked IsChecked { get; private set; }
    public ActivityRuleId? ParentActivityRuleId { get; private set; }

    /// <summary>
    /// <remarks>Use only for Mongo purposes</remarks>
    /// </summary>
    public Activity(ActivityId id, Details details, IsChecked isChecked, 
        ActivityRuleId? parentActivityRuleId) : base(id)    
    {
        Details = details;
        IsChecked = isChecked;
        ParentActivityRuleId = parentActivityRuleId;
    }

    internal static Activity Create(ActivityId activityId, string title, string description,
        ActivityRuleId? parentActivityRuleId)
    {
        var details = Details.Create(title, description);
        return new Activity(activityId, details, true, parentActivityRuleId);
    }
}