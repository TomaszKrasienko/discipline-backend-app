using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class ActivityRule
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    private List<SelectedDay> _selectedDays = [];
    public IReadOnlyList<SelectedDay> SelectedDays => _selectedDays; 

    private ActivityRule(EntityId id)
        => Id = id;

    //For mongo
    public ActivityRule(EntityId id, Title title, Mode mode, IEnumerable<SelectedDay> selectedDays)
    {
        Id = id;
        Title = title;
        Mode = mode;
        _selectedDays = selectedDays?.ToList() ?? [];
    }

    internal static ActivityRule Create(Guid id, string title, string mode, List<int> selectedDays = null)
    {
        var item = new ActivityRule(id);
        item.ChangeTitle(title);
        item.ChangeMode(mode);
        item.ChangeSelectedDays(mode, selectedDays);
        return item;
    }

    internal void Edit(string title, string mode, List<int> selectedDays = null)
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