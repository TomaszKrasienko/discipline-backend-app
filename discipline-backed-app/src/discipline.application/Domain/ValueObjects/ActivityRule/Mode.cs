using discipline.application.Domain.Exceptions;

namespace discipline.application.Domain.ValueObjects.ActivityRule;

internal sealed record Mode
{
    private readonly Dictionary<string, string> _availableModes = new Dictionary<string, string>
    {
        ["EveryDay"] = "Every day",
        ["OnceAWeek"] = "Once a week", 
        ["FirstDayOfWeek"] = "First day of week", 
        ["LastDayOfWeek"] = "Last day of week", 
        ["FirstDayOfMonth"] = "First day of month",
        ["LastDayOfMonth"] = "Last day of month",
        ["Custom"] = "Custom"
    };

    public string Value { get; }

    internal Mode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityRuleModeException();
        }

        if (!_availableModes.ContainsKey(value))
        {
            throw new ModeUnavailableException(value);
        }
        Value = value;
    }

    public static implicit operator string(Mode mode)
        => mode.Value;

    public static implicit operator Mode(string value)
        => new Mode(value);
}