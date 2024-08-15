namespace discipline.domain.DailyProductivities.ValueObjects.Activity;

public sealed record IsChecked
{
    public bool Value { get; }

    public IsChecked(bool value)
        => Value = value;

    public static implicit operator bool(IsChecked isChecked)
        => isChecked.Value;

    public static implicit operator IsChecked(bool value)
        => new IsChecked(value);
}