using discipline.domain.SharedKernel;

namespace discipline.domain.DailyProductivities.ValueObjects.Activity;

public sealed class IsChecked : ValueObject
{
    public bool Value { get; }

    public IsChecked(bool value)
        => Value = value;

    public static implicit operator bool(IsChecked isChecked)
        => isChecked.Value;

    public static implicit operator IsChecked(bool value)
        => new IsChecked(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}