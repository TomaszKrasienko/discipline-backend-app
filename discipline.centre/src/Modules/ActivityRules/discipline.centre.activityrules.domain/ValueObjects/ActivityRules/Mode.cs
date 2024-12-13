using System.Collections.Immutable;
using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.Rules.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

public sealed class Mode : ValueObject
{
    private readonly string _value = null!;
    public static readonly ImmutableDictionary<string, string> AvailableModes = new Dictionary<string, string>()
    {
        ["EveryDay"] = "Every day",
        ["FirstDayOfWeek"] = "First day of week",
        ["LastDayOfWeek"] = "Last day of week",
        ["FirstDayOfMonth"] = "First day of month",
        ["LastDayOfMonth"] = "Last day of month",
        ["Custom"] = "Custom"
    }.ToImmutableDictionary();

    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new ModeCanNotBeEmptyRule(value));
            CheckRule(new ModeCanNotBeUnavailableRule(value, AvailableModes.Keys.ToImmutableList()));
            _value = value;
        }
    }

    private Mode(string value) => Value = value;

    public static Mode Create(string value) => new Mode(value);
    
    public static Mode EveryDayMode => "EveryDay";
    
    public static Mode FirstDayOfWeekMode => "FirstDayOfWeek";
    
    public static Mode LastDayOfWeekMode => "LastDayOfWeek";

    public static Mode CustomMode => "Custom";

    public static Mode FirstDayOfMonth => "FirstDayOfMonth";
    
    public static Mode LastDayOfMonthMode => "LastDayOfMonth";

    public static implicit operator string(Mode mode) => mode.Value;

    public static implicit operator Mode(string value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}