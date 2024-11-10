using discipline.centre.activityrules.domain.Rules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects;

public sealed class SelectedDay : ValueObject
{
    private readonly int _value;

    public int Value
    {
        get => _value;
        private init
        {
            CheckRule(new SelectedDayCanNotBeOutOfRangeRule(value));
            _value = value;
        }
    }

    private SelectedDay(int value) => Value = value;
    
    public static SelectedDay Create(int value) => new SelectedDay(value);

    public static implicit operator int(SelectedDay day) => day.Value;

    public static implicit operator SelectedDay(int value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}