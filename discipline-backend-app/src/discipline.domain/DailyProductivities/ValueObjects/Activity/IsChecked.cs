namespace discipline.domain.DailyProductivities.ValueObjects.Activity;

internal sealed record IsChecked
{
    internal bool Value { get; }

    internal IsChecked(bool value)
        => Value = value;

    public static implicit operator bool(IsChecked isChecked)
        => isChecked.Value;

    public static implicit operator IsChecked(bool value)
        => new IsChecked(value);
}