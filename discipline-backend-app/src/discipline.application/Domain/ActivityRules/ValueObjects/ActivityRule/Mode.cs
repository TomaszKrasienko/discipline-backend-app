using discipline.application.Domain.ActivityRules.Exceptions;

namespace discipline.application.Domain.ActivityRules.ValueObjects.ActivityRule;

internal sealed record Mode
{
    internal static readonly Dictionary<string, string> AvailableModes = new Dictionary<string, string>
    {
        ["EveryDay"] = "Every day",
        ["FirstDayOfWeek"] = "First day of week", 
        ["LastDayOfWeek"] = "Last day of week", 
        ["FirstDayOfMonth"] = "First day of month",
        ["LastDayOfMonth"] = "Last day of month",
        ["Custom"] = "Custom"
    };

    internal string Value { get; }

    internal Mode(string value)
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