using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.Extensions.Options;

namespace discipline.centre.activityrules.domain;

public sealed class ActivityRule : AggregateRoot<ActivityRuleId> 
{
    private List<SelectedDay>? _selectedDays;
    public UserId UserId { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    
    public IReadOnlyList<SelectedDay>? SelectedDays => _selectedDays;

    /// <summary>
    /// Constructor for mapping to mongo documents
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="mode"></param>
    /// <param name="selectedDays"></param>
    public ActivityRule(ActivityRuleId id, UserId userId, Title title,
        Mode mode, List<SelectedDay>? selectedDays) : base(id)
    {        
        UserId = userId;
        Title = title;
        Mode = mode;
        _selectedDays = selectedDays;
    }

    public static ActivityRule Create(ActivityRuleId id, UserId userId, string title, string mode, List<int>? selectedDays = null)
    {
        Validate(mode, selectedDays);
        var days = ConvertSelectedDays(selectedDays);

        return new ActivityRule(id, userId, title, mode, days);
    }

    public void Edit(string title, string mode, List<int>? selectedDays = null)
    {
        Validate(mode, selectedDays);
        var days = ConvertSelectedDays(selectedDays);
        Title = title;
        Mode = mode;
        _selectedDays = days;
    }

    private static List<SelectedDay>? ConvertSelectedDays(List<int>? selectedDays)
    {
        List<SelectedDay> days = null!;
        if (selectedDays is not null)
        {
            days = selectedDays.Select(SelectedDay.Create).ToList();
        }

        return days;
    }
    
    private static void Validate(string mode, List<int>? selectedDays)
    {
        CheckRule(new ModeCannotHaveFilledSelectedDays(mode, selectedDays));
        CheckRule(new ModeMustHaveFilledSelectedDays(mode, selectedDays));   
    }

    private void ChangeTitle(string value)
        => Title = value;

    private void ChangeMode(string value)
        => Mode = value;

    private static bool IsSelectedDaysNullOrEmpty(List<int>? selectedDays)
        => selectedDays is null || selectedDays.Count == 0;

}