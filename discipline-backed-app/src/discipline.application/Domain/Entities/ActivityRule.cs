using discipline.application.Domain.Exceptions;
using discipline.application.Domain.ValueObjects.ActivityRules;
using discipline.application.Domain.ValueObjects.SharedKernel;

namespace discipline.application.Domain.Entities;

internal sealed class ActivityRule
{
    public EntityId Id { get; }
    public Title Title { get; private set; }
    public Mode Mode { get; private set; }
    private readonly List<SelectedDay> _selectedDays = new List<SelectedDay>();
    public IReadOnlyList<SelectedDay> SelectedDays => _selectedDays; 

    private ActivityRule(EntityId id)
        => Id = id;

    internal static ActivityRule Create(Guid id, string title, string mode, List<int> selectedDays = null)
    {
        var item = new ActivityRule(id);
        item.ChangeTitle(title);
        item.ChangeMode(mode);
        if (!(selectedDays is null || selectedDays.Count == 0))
        {
            item.ChangeSelectedDays(mode, selectedDays);
        }

        return item;
    }

    private void ChangeTitle(string value)
        => Title = value;

    private void ChangeMode(string value)
        => Mode = value;

    private void ChangeSelectedDays(string mode, List<int> selectedDays)
    {
        if (mode != Mode.CustomMode())
        {
            throw new InvalidModeForSelectedDaysException(mode);
        }

        foreach (var selectedDay in selectedDays)
        {
            _selectedDays.Add(selectedDay);
        }
    }
}