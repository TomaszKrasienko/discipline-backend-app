using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests;

public sealed class ActivityRuleTests
{
    [Theory]
    [MemberData(nameof(GetValidCreateActivityRulesData))]
    public void Create_GivenValidaArguments_ShouldReturnActivityRuleWithValues(ActivityRuleParams @params)
    {
        //act
        var result = ActivityRule.Create(@params.Id!, @params.UserId!, @params.Title,
            @params.Mode, @params.SelectedDays);
        
        //assert
        result.Id.ShouldBe(@params.Id);
        result.UserId.ShouldBe(@params.UserId);
        result.Title.Value.ShouldBe(@params.Title);
        result.Mode.Value.ShouldBe(@params.Mode);
        CompareSelectedDays(@params.SelectedDays, result.SelectedDays).ShouldBeTrue();
    }

    public static IEnumerable<object[]> GetValidCreateActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                Mode.EveryDayMode, null)
        ];

        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                Mode.CustomMode, [1,2,3])
        ];
    }

    [Theory]
    [MemberData(nameof(GetInvaliCreateActivityRulesData))]
    public void Create_GivenInvalidArgument_ShouldReturnDomainExceptionWithCode(ActivityRuleParams @params, string code)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(@params.Id!, @params.UserId!, @params.Title,
            @params.Mode, @params.SelectedDays));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    public static IEnumerable<object[]> GetInvaliCreateActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [1, 2, 3]),
            "ActivityRule.Title.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                string.Empty, [1, 2, 3]),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), "test_title",
                "test_mode", [1, 2, 3]),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new ActivityRuleParams(ActivityRuleId.New(), UserId.New(), string.Empty,
                Mode.CustomMode, [1, 7, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
    }
    
    [Theory]
    [MemberData(nameof(GetValidModesForSelectedDays))]
    public void Create_GivenModeForSelectedDaysAndNullSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title",
            mode, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.RequireSelectedDays");
    }

    public static IEnumerable<object[]> GetValidModesForSelectedDays()
    {
        yield return [Mode.CustomMode];
    }

    [Theory]
    [MemberData(nameof(GetInvalidModesForSelectedDays))]
    public void Create_GivenInvalidModeForSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title",
            mode, [1,2,3]));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.RequireSelectedDays");
    }

    public static IEnumerable<object[]> GetInvalidModesForSelectedDays()
    {
        yield return [Mode.EveryDayMode];
        yield return [Mode.FirstDayOfWeekMode];
        yield return [Mode.LastDayOfWeekMode];
        yield return [Mode.FirstDayOfMonth];
        yield return [Mode.LastDayOfMonthMode];
    }
    
    [Theory]
    [MemberData(nameof(GetValidEditActivityRulesData))]
    public void Edit_GivenValidaArguments_ShouldChangeActivityRule(ActivityRuleParams @params)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        activityRule.Edit(@params.Title, @params.Mode, @params.SelectedDays);
        
        //assert
        activityRule.Title.Value.ShouldBe(@params.Title);
        activityRule.Mode.Value.ShouldBe(@params.Mode);
        CompareSelectedDays(@params.SelectedDays, activityRule.SelectedDays).ShouldBeTrue();
    }

    public static IEnumerable<object[]> GetValidEditActivityRulesData()
    {
        yield return
        [
            new ActivityRuleParams(null, null, "test_title", Mode.EveryDayMode, null)
        ];

        yield return
        [
            new ActivityRuleParams(null, null, "test_title", Mode.CustomMode, [1,2,3])
        ];
    }
    
    public sealed record ActivityRuleParams(ActivityRuleId? Id, UserId? UserId, string Title, string Mode,
        List<int>? SelectedDays = null);

    private static bool CompareSelectedDays(List<int>? provided, IReadOnlyList<SelectedDay>? src)
    {
        if(provided is null && src is null)
        {
            return true;
        }

        if ((provided is null && src is not null) || (provided is not null && src is null))
        {
            return false;
        }

        foreach (var srcDay in src!)
        {
            if (!provided!.Contains(srcDay.Value))
            {
                return false;
            }
        }

        return true;
    }
}    