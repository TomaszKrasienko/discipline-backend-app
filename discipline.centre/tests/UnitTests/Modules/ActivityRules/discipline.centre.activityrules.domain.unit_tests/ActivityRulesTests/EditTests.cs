using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class EditTests
{
    [Theory]
    [MemberData(nameof(GetValidEditActivityRulesData))]
    public void Edit_GivenValidaArguments_ShouldChangeActivityRule(EditActivityRuleParams @params)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        activityRule.Edit(@params.Title, @params.Note, @params.Mode, @params.SelectedDays);
        
        //assert
        activityRule.Details.Title.ShouldBe(@params.Title);
        activityRule.Details.Note.ShouldBe(@params.Note);
        activityRule.Mode.Value.ShouldBe(@params.Mode);
        CompareSelectedDays(@params.SelectedDays, activityRule.SelectedDays).ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(GetEditChangedParameters))]
    public void Edit_HasAtLeastOneChangedParameter_ShouldNotThrowException(ActivityRule activityRule, EditActivityRuleParams @params)
    {
        //act
        var exception = Record.Exception(() => activityRule.Edit(@params.Title, @params.Note, @params.Mode, 
            @params.SelectedDays));
        
        //assert
        exception.ShouldBeNull();
    }
    
    [Theory]
    [MemberData(nameof(GetEditUnchangedParameters))]
    public void Edit_HasUnchangedParameters_ShouldThrowDomainExceptionWithCode(ActivityRule activityRule, EditActivityRuleParams @params)
    {
        //act
        var exception = Record.Exception(() => activityRule.Edit(@params.Title, @params.Note, @params.Mode, @params.SelectedDays));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.NoChanges");
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidEditActivityRulesData))]
    public void Edit_GivenInvalidArgument_ShouldReturnDomainExceptionWithCode(EditActivityRuleParams @params, string code)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        var exception = Record.Exception(() => activityRule.Edit( @params.Title, @params.Note, @params.Mode, @params.SelectedDays));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    [Theory]
    [MemberData(nameof(GetValidModesForSelectedDays))]
    public void Edit_GivenModeForSelectedDaysAndNullSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        var exception = Record.Exception(() => activityRule.Edit("test_title", null,
            mode, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.RequireSelectedDays");
    }

    [Theory]
    [MemberData(nameof(GetInvalidModesForSelectedDays))]
    public void Edit_GivenInvalidModeForSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();
        
        //act
        var exception = Record.Exception(() => activityRule.Edit("test_title", null, mode, [1,2,3]));
        
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

