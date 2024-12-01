using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class CreateTests
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

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRulesData))]
    public void Create_GivenInvalidArgument_ShouldReturnDomainExceptionWithCode(ActivityRuleParams @params, string code)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(@params.Id!, @params.UserId!, @params.Title,
            @params.Mode, @params.SelectedDays));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
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
 
    private static bool CompareSelectedDays(List<int>? provided, SelectedDays? src)
    {
        if(provided is null && src is null)
        {
            return true;
        }

        return provided?.Count == src?.Values.Count 
               && provided!.OrderBy(x => x).SequenceEqual(src!.Values.OrderBy(x => x).Select(x => (int)x));
    }
}