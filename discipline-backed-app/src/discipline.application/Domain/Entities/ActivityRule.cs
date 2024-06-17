using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class ActivityRule
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    private List<SelectedDay> _selectedDays = new List<SelectedDay>();
    public IReadOnlyList<SelectedDay> SelectedDays => _selectedDays; 

    private ActivityRule(EntityId id)
        => Id = id;

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
        ChangeMode(title);
        ChangeSelectedDays(Mode, selectedDays);
    }

    private void ChangeTitle(string value)
        => Title = value;

    private void ChangeMode(string value)
        => Mode = value;

    private void ChangeSelectedDays(string mode, List<int> selectedDays)
    {
        if (mode == Mode.CustomMode() && !IsSelectedDaysNullOrEmpty(selectedDays))
        {
            throw new InvalidModeForSelectedDaysException(mode);
        }

        if (IsSelectedDaysNullOrEmpty(selectedDays))
        {
            _selectedDays = null;
        }
        
        foreach (var selectedDay in selectedDays)
        {
            _selectedDays = new();
            _selectedDays.Add(selectedDay);
        }
    }

    private static bool IsSelectedDaysNullOrEmpty(List<int> selectedDays)
        => selectedDays is null || selectedDays.Count == 0;

}