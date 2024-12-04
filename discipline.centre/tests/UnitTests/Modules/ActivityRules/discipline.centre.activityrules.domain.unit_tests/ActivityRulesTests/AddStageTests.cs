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
    public void ShouldAddStage_WhenStageIsNotNull()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), Mode.EveryDayMode, 
            null, [new StageSpecification("test_stage_title1", 1)]);

        var title = "test_stage_title2";
        var index = 2;
        
        //act
        activityRule.AddStage(new StageSpecification(title, index));
        
        //assert
        activityRule.Stages.ShouldNotBeNull();
        activityRule.Stages[1].Title.Value.ShouldBe(title);
        activityRule.Stages[1].Index.Value.ShouldBe(index);
    }
    
    [Fact]
    public void ShouldCreateStagesListAndAddStage_WhenStageIsNull()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), Mode.EveryDayMode,
            null, null);

        var title = "test_stage_title";
        var index = 1;
        
        //act
        activityRule.AddStage(new StageSpecification(title, index));
        
        //assert
        activityRule.Stages.ShouldNotBeNull();
        activityRule.Stages[0].Title.Value.ShouldBe(title);
        activityRule.Stages[0].Index.Value.ShouldBe(index);
    }
    
    [Fact]
    public void ShouldThrowDomainExceptionWithCodeActivityRuleStagesMustHaveOrderedIndex_WhenStageHasInvalidIndex()
    {
        //arrange
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), Mode.EveryDayMode, 
            null, [new StageSpecification("test_stage1", 1)]);
        
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
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), UserId.New(), 
            new ActivityRuleDetailsSpecification("test_title", null), Mode.EveryDayMode, 
            null, [new StageSpecification("test_stage1", 1)]);
        
        //act
        var exception = Record.Exception(() => activityRule.AddStage(new StageSpecification("test_stage1", 2)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    }
}