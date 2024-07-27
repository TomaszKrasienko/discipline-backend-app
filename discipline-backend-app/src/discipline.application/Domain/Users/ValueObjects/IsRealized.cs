namespace discipline.application.Domain.Users.ValueObjects;

internal sealed record IsRealized(bool Value)
{
    public static implicit operator bool(IsRealized isRealized)
        => isRealized.Value;

    public static implicit operator IsRealized(bool value)
        => new IsRealized(value);
}