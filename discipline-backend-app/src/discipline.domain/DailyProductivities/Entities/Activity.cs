using discipline.domain.ActivityRules.Entities;
using discipline.domain.DailyProductivities.Services.Internal;
using discipline.domain.DailyProductivities.ValueObjects.Activity;
using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.Entities;

public sealed class Activity : Entity<Ulid>
{
    public Title Title { get; private set; }
    public IsChecked IsChecked { get; private set; }
    public Ulid ParentRuleId { get; private set; }

    private Activity(Ulid id) : base(id)
        => IsChecked = false;
    
    private Activity(Ulid id, Ulid parentRuleId) : this(id)
        => ParentRuleId = parentRuleId;
    
    //For mongo
    public Activity(Ulid id, Title title, IsChecked isChecked, Ulid parentRuleId) : this(id)
    {
        Title = title;
        IsChecked = isChecked;
        ParentRuleId = parentRuleId;
    }

    internal static Activity Create(Ulid id, string title)
    {
        var activity = new Activity(id);
        activity.ChangeTitle(title);
        return activity;
    }

    internal static Activity CreateFromRule(Ulid id, DateTime now, ActivityRule rule)
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