using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class MarkStageAsChecked
{
    [Fact]
    public void ShouldChangeIsCheckedOfPrcisedStageAndNotCheckedActivity_WhenCheckedCorrectlyOnlyOneOfStages()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title_1", 1),
            new StageSpecification("test_stage_title_2", 2)]);
        var stage = activity.Stages!.First();

        //act
        activity.MarkStageAsChecked(stage!.Id);
        
        //assert
        activity.IsChecked.Value.ShouldBeFalse();
    }
    
    [Fact]
    public void ShouldChangeIsCheckedOfStage_WhenStageExists()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        var stage = activity.Stages!.Single();

        //act
        activity.MarkStageAsChecked(stage!.Id);
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public void ShouldChangeIsCheckedOfStageAndIsCheckedOfActivity_WhenLastStageMarkedAsChecked()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        var stage = activity.Stages!.Single();

        //act
        activity.MarkStageAsChecked(stage!.Id);
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
        activity.IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public void ShouldThrowDomainException_WhenStageIsNotExists()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        
        //act
        var exception = Record.Exception(() => activity.MarkStageAsChecked(StageId.New()));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.StageNotFound");
    }
}