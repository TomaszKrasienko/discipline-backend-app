using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public sealed class AddStageTests
{
    [Fact]
    public void ShouldThrowDomainExceptionWithCodeActivityRuleStagesMustHaveOrderedIndex_WhenStageHasInvalidIndex()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title",
            null, Mode.EveryDayMode, null, [new StageSpecification("test_stage1", 1)]);
        
        //act
        var exception = Record.Exception(() => activityRule.AddStage(new StageSpecification("test_stage3", 3)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.MustHaveOrderedIndex");
    }
    
    [Fact]
    public void ShouldThrowDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique_WhenStageHasNotUniqueTitle()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), "test_title",
            null, Mode.EveryDayMode, null, [new StageSpecification("test_stage1", 1)]);
        
        //act
        var exception = Record.Exception(() => activityRule.AddStage(new StageSpecification("test_stage1", 2)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    }
}