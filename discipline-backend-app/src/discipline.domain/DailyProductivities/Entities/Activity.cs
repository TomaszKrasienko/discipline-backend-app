using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Services.Internal;
using discipline.domain.DailyProductivities.ValueObjects.Activity;
using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.DailyProductivities.Entities;

public sealed class Activity : Entity<ActivityId>
{
    public Title Title { get; private set; }
    public IsChecked IsChecked { get; private set; }
    public ActivityRuleId ParentRuleId { get; private set; }

    private Activity(ActivityId id) : base(id)
        => IsChecked = false;
    
    private Activity(ActivityId id, ActivityRuleId parentRuleId) : this(id)
        => ParentRuleId = parentRuleId;
    
    //For mongo
    public Activity(ActivityId id, Title title, IsChecked isChecked, ActivityRuleId parentRuleId) : this(id)
    {
        Title = title;
        IsChecked = isChecked;
        ParentRuleId = parentRuleId;
    }

    internal static Activity Create(ActivityId id, string title)
    {
        var activity = new Activity(id);
        activity.ChangeTitle(title);
        return activity;
    }

    internal static Activity CreateFromRule(ActivityId id, DateTime now, ActivityRule rule)
    {
        var weekdayCheckService = WeekdayCheckService.GetInstance();
        if (weekdayCheckService.IsDateForMode(now, rule.Mode, rule.SelectedDays?.Select(x => x.Value).ToList()))
        {
            var activity = new Activity(id, rule.Id);
            activity.ChangeTitle(rule.Title);
            return activity;
        }
        return null;
    }

    private void ChangeTitle(string value)
        => Title = value;

    internal void ChangeCheck()
        => IsChecked = !IsChecked;
}