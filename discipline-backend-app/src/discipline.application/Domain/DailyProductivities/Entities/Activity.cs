using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.DailyProductivities.Services.Factories;
using discipline.application.Domain.DailyProductivities.ValueObjects.Activity;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.DailyProductivities.Entities;

internal sealed class Activity
{
    internal EntityId Id { get; }
    internal Title Title { get; private set; }
    internal IsChecked IsChecked { get; private set; }
    internal EntityId ParentRuleId { get; private set; }

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
        var weekdayCheckService = WeekdayCheckServiceFactory.GetInstance();
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