using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public sealed class DeleteActivityStageTests
{
    [Fact]
    public void GivenExistingActivityStage_WhenDeleteActivityStage_ThenShouldRemoveStage()
    {
        // Arrange
        var activityId = ActivityId.New();
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025,1,1), 
            UserId.New(), activityId,  new ActivityDetailsSpecification("test_activity_title", null), null,
            [new StageSpecification("test_title1", 1), new StageSpecification("test_title2", 2)]);
        
        var stage1 = dailyTracker
            .Activities
            .Single(x => x.Id! == activityId)
            .Stages!.Single(x => x.Index.Value == 1);
        
        // Act
        dailyTracker.DeleteActivityStage(activityId, stage1.Id);
        
        // Assert
        dailyTracker.Activities.Single().Stages!.Count.ShouldBe(1);
        dailyTracker.Activities.Single().Stages!.Any(x => x.Index.Value == 1).ShouldBeTrue();
    }
}