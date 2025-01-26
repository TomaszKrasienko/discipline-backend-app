using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public sealed class MarkActivityStageAsCheckedTests
{
    [Fact]
    public void ShouldMarkStageAsChecked_WhenExistingActivity()
    {
        //arrange
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.Now),
            UserId.New(), ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        var activity = dailyTracker.Activities.Single();
        var stage = activity.Stages!.Single();
        
        //act
        dailyTracker.MarkActivityStageAsChecked(activity.Id, stage.Id);
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public void ShouldMarkStageAsChecked_WhenNotExistingActivity()
    {
        //arrange
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.Now),
            UserId.New(), ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        
        //act
        var exception = Record.Exception(() => dailyTracker.MarkActivityStageAsChecked(ActivityId.New(), StageId.New()));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
}