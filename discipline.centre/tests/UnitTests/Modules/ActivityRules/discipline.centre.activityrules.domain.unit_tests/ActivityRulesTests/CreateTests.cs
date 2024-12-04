using discipline.centre.activityrules.domain.Specifications;
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
    public void GivenValidaArguments_ShouldReturnActivityRuleWithValues(CreateActivityRuleParams parameters)
    {
        //act
        var result = ActivityRule.Create(parameters.Id!, parameters.UserId!, parameters.Details, 
            parameters.Mode, parameters.SelectedDays, parameters.Stages);
        
        //assert
        result.Id.ShouldBe(parameters.Id);
        result.UserId.ShouldBe(parameters.UserId);
        result.Details.Title.ShouldBe(parameters.Details.Title);
        result.Details.Note.ShouldBe(parameters.Details.Note);
        result.Mode.Value.ShouldBe(parameters.Mode);
        CompareSelectedDays(parameters.SelectedDays, result.SelectedDays).ShouldBeTrue();
        CompareStages(parameters.Stages, result.Stages?.ToList()).ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRulesData))]
    public void GivenInvalidArgument_ShouldReturnDomainExceptionWithCode(CreateActivityRuleParams parameters, string code)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(parameters.Id!, parameters.UserId!, parameters.Details,
            parameters.Mode, parameters.SelectedDays, parameters.Stages));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }
    
    [Theory]
    [MemberData(nameof(GetValidModesForSelectedDays))]
    public void GivenModeForSelectedDaysAndNullSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), mode, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Mode.RequireSelectedDays");
    }

    [Theory]
    [MemberData(nameof(GetInvalidModesForSelectedDays))]
    public void GivenInvalidModeForSelectedDays_ShouldThrowDomainExceptionWithCode(string mode)
    {
        //act
        var exception = Record.Exception(() => ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), mode, [1,2,3]));
        
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

    private static bool CompareStages(List<StageSpecification>? provided, List<Stage>? src)
    {
        if(provided is null && src is null)
        {
            return true;
        }

        if ((src is null && provided is not null) || (provided is null && src is not null))
        {
            return false;
        }

        foreach (var srcStage in src!)
        {
            if (!(provided!.Exists(x => x.Index == srcStage.Index
                                   && x.Title == srcStage.Title)))
            {
                return false;
            }
        }
        return true;
    }
}