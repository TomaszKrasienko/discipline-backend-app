namespace discipline.application.Domain.ValueObjects.ActivityRule;

public sealed record Title
{
    public string Value { get; }

    public Title(string value)
    {
        Value = value;
    }
}