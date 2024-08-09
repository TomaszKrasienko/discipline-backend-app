using discipline.domain.ActivityRules.Exceptions;

namespace discipline.domain.ActivityRules.ValueObjects.ActivityRule;

public sealed record Mode
{
    public static readonly Dictionary<string, string> AvailableModes = new Dictionary<string, string>
    {
        ["EveryDay"] = "Every day",
        ["FirstDayOfWeek"] = "First day of week", 
        ["LastDayOfWeek"] = "Last day of week", 
        ["FirstDayOfMonth"] = "First day of month",
        ["LastDayOfMonth"] = "Last day of month",
        ["Custom"] = "Custom"
    };

    public string Value { get; }

    public Mode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyActivityRuleModeException();
        }

        if (!AvailableModes.ContainsKey(value))
        {
            throw new ModeUnavailableException(value);
        }
        Value = value;
    }

    internal static string EveryDayMode()
        => "EveryDay";
    
    internal static string FirstDayOfWeekMode()
        => "FirstDayOfWeek";
    
    internal static string LastDayOfWeekMode()
        => "LastDayOfWeek";

    internal static string CustomMode()
        => "Custom";

    internal static string FirstDayOfMonth()
        => "FirstDayOfMonth";
    
    internal static string LastDayOfMonthMode()
        => "LastDayOfMonth";

    public static implicit operator string(Mode mode)
        => mode.Value;

    public static implicit operator Mode(string value)
        => new Mode(value);
}