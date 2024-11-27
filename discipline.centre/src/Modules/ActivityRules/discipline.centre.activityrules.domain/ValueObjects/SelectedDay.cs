using discipline.centre.activityrules.domain.Rules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects;

public sealed class SelectedDays : ValueObject
{
    private readonly List<DayOfWeek> _values;
    public IReadOnlyCollection<DayOfWeek> Values => _values;
    
    private SelectedDays(List<DayOfWeek> values) => _values = values;

    public static SelectedDays Create(List<int> values)
    {
        foreach (var value in values)
        {
            CheckRule(new SelectedDayCanNotBeOutOfRangeRule(value));
        }

        return new SelectedDays(values.Select(x => (DayOfWeek)x).ToList());
    }

    internal bool HasChanges(List<int>? days)
        => days is null || !_values.Select(x => (int)x).SequenceEqual(days);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Values;
    }
}