using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class DeleteStageTests
{
    [Fact]
    public void ShouldNotThrowException_WhenStageDoesNotExist()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1)]);
        var stage = activity.Stages!.First();
        
        //act
        var exception = Record.Exception(() => activity.DeleteStage(StageId.New()));
        
        //assert
        exception.ShouldBeNull();   
    }
    
    [Fact]
    public void ShouldRemoveStageAndSetValidIndexes_WhenStageExists()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1),
                new StageSpecification("test_stage_title2", 2)]);
        var stage = activity.Stages!.First();
        
        //act
        activity.DeleteStage(stage.Id);
        
        //assert
        activity.Stages!.Any(x => x.Id == stage.Id).ShouldBeFalse();
        activity.Stages!.First().Index.Value.ShouldBe(1);
    }
    
    [Fact]
    public void ShouldRemoveStageAndSetNull_WhenStageExistsAndWasOnlyOne()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1)]);
        var stage = activity.Stages!.First();
        
        //act
        activity.DeleteStage(stage.Id);
        
        //assert
        activity.Stages.ShouldBeNull();
    }
}