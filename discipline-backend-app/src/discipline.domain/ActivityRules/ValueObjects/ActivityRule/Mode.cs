using discipline.domain.ActivityRules.Exceptions;
using discipline.domain.SharedKernel;

namespace discipline.domain.ActivityRules.ValueObjects.ActivityRule;

public sealed class Mode : ValueObject
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

    public static string EveryDayMode()
        => "EveryDay";
    
    public static string FirstDayOfWeekMode()
        => "FirstDayOfWeek";
    
    public static string LastDayOfWeekMode()
        => "LastDayOfWeek";

    public static string CustomMode()
        => "Custom";

    public static string FirstDayOfMonth()
        => "FirstDayOfMonth";
    
    public static string LastDayOfMonthMode()
        => "LastDayOfMonth";

    public static implicit operator string(Mode mode)
        => mode.Value;

    public static implicit operator Mode(string value)
        => new Mode(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}