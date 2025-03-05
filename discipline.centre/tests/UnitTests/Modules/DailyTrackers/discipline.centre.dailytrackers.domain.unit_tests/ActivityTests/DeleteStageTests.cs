using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.ActivityTests;

public sealed class DeleteStageTests
{
    [Fact]
    public void GivenExistingStage_WhenDeleteStage_ThenShouldRemoveStageAndSetValidIndexes()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1),
                new StageSpecification("test_stage_title2", 2)]);
        var stage = activity.Stages!.First();
        
        // Act
        _ = activity.DeleteStage(stage.Id);
        
        // Assert
        activity.Stages!.Any(x => x.Id == stage.Id).ShouldBeFalse();
        activity.Stages!.First().Index.Value.ShouldBe(1);
    }
    
    [Fact]
    public void GivenExistingStage_WhenDeleteStage_ThenReturnTrue()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1),
                new StageSpecification("test_stage_title2", 2)]);
        var stage = activity.Stages!.First();
        
        // Act
        var result = activity.DeleteStage(stage.Id);
        
        // Assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void GivenOnlyOneStage_WhenDeleteStage_ThenShouldRemoveStageAndSetNull()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1)]);
        var stage = activity.Stages!.First();
        
        // Act
        _ = activity.DeleteStage(stage.Id);
        
        // Assert
        activity.Stages.ShouldBeNull();
    }
        
    [Fact]
    public void GivenOnlyOneStage_WhenDeleteStage_ThenReturnTrue()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1)]);
        var stage = activity.Stages!.First();
        
        // Act
        var result = activity.DeleteStage(stage.Id);
        
        // Assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void GivenNonExistingStage_WhenDeleteStage_ThenReturnFalse()
    {
        // Arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title1", 1)]);
        
        // Act
        var result = activity.DeleteStage(StageId.New());
        
        // Assert
        result.ShouldBeFalse();
    }
}