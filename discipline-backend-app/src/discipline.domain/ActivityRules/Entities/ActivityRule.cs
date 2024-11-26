using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.ActivityRules.ValueObjects.ActivityRule;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.SharedKernel;
using discipline.domain.SharedKernel.TypeIdentifiers;

namespace discipline.domain.ActivityRules.Entities;

public sealed class ActivityRule : Entity<ActivityRuleId> 
{
    public UserId UserId { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    private List<SelectedDay> _selectedDays = [];
    public IReadOnlyList<SelectedDay> SelectedDays => _selectedDays;

    private ActivityRule(ActivityRuleId id, UserId userId) : base(id)
        => UserId = userId;

    //For mongo
    public ActivityRule(ActivityRuleId id, UserId userId, Title title, 
        Mode mode, IEnumerable<SelectedDay> selectedDays) : this(id, userId)
    {
        Title = title;
        Mode = mode;
        _selectedDays = selectedDays?.ToList() ?? [];
    }

    public static ActivityRule Create(ActivityRuleId id, UserId userId, string title, string mode, List<int> selectedDays = null)
    {
        var item = new ActivityRule(id, userId);
        item.ChangeTitle(title);
        item.ChangeMode(mode);
        item.ChangeSelectedDays(mode, selectedDays);
        return item;
    }

    public void Edit(string title, string mode, List<int> selectedDays = null)
    {
        ChangeTitle(title);
        ChangeMode(mode);
        ChangeSelectedDays(mode, selectedDays);
    }

    private void ChangeTitle(string value)
        => Title = value;

    private void ChangeMode(string value)
        => Mode = value;

    private void ChangeSelectedDays(string mode, List<int> selectedDays)
    {
        if (mode == Mode.CustomMode() && !IsSelectedDaysNullOrEmpty(selectedDays))
        {
            _selectedDays = new();
            foreach (var selectedDay in selectedDays)
            {
                _selectedDays.Add(selectedDay);
            }
        }

        if(mode == Mode.CustomMode() && IsSelectedDaysNullOrEmpty(selectedDays) || mode != Mode.CustomMode() && !IsSelectedDaysNullOrEmpty(selectedDays))            
        {
            throw new InvalidModeForSelectedDaysException(mode);
        }

        if (IsSelectedDaysNullOrEmpty(selectedDays))
        {
            _selectedDays = null;
        }
        

    }

    private static bool IsSelectedDaysNullOrEmpty(List<int> selectedDays)
        => selectedDays is null || selectedDays.Count == 0;

}