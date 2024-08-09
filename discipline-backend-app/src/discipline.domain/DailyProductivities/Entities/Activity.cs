using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Services.Internal;
using discipline.domain.DailyProductivities.ValueObjects.Activity;
using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Entities;

public sealed class Activity
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public IsChecked IsChecked { get; private set; }
    public EntityId ParentRuleId { get; private set; }

    private Activity(EntityId id)
    {
        Id = id;
        IsChecked = false;
    }

    private Activity(EntityId id, EntityId parentRuleId) : this(id)
    {
        ParentRuleId = parentRuleId;
    }
    
    //For mongo
    internal Activity(EntityId id, Title title, IsChecked isChecked, EntityId parentRuleId)
    {
        Id = id;
        Title = title;
        IsChecked = isChecked;
        ParentRuleId = parentRuleId;
    }

    internal static Activity Create(Guid id, string title)
    {
        var activity = new Activity(id);
        activity.ChangeTitle(title);
        return activity;
    }

    internal static Activity CreateFromRule(Guid id, DateTime now, ActivityRule rule)
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