using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class AddStageTests
{
    [Fact]
    public void ShouldAddNewStage_WhenTitleIsUnique()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
            null), null, null);
        
        //act
        _ = activity.AddStage("Test_title");
        
        //assert
        activity.Stages!.Any().ShouldBeTrue();
    } 
    
    [Fact]
    public void ShouldReturnStageWithIndex1_WhenItIsFirstStage()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
            null), null, null);
        
        //act
        var stage = activity.AddStage("Test_title");
        
        //assert
        stage.Index.Value.ShouldBe(1);
    } 
    
    [Fact]
    public void ShouldReturnStageWithIndexAsNext_WhenItIsNotFirstStage()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
            null), null, null);
        
        _ = activity.AddStage("Test_title");
        
        //act
        var stage = activity.AddStage("Test_title_second");
        
        //assert
        stage.Index.Value.ShouldBe(2);
    } 
    
    [Fact]
    public void ShouldThrowDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique_WhenTitleIsNotUnique()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
            null), null, null);
        var title = "test_stage_title";
        
        activity.AddStage(title);
        
        //act
       var exception = Record.Exception(() => activity.AddStage(title));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    } 
}