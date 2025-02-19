using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public sealed class MarkActivityAsCheckedShould
{
    [Fact]
    public void MarkActivityAsChecked_WhenActivityExists()
    {
        // Arrange
        var activityId = ActivityId.New();
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 1),
            UserId.New(), activityId, new ActivityDetailsSpecification("test_title", null), null,
            null);
        
        // Act
        dailyTracker.MarkActivityAsChecked(activityId);
        
        // Assert
        dailyTracker.Activities.Single(x => x.Id == activityId).IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public void ShouldThrowDomainExceptionWithCodeDailyTrackerActivityNotExists_WhenActivityExists()
    {
        // Arrange
        var activityId = ActivityId.New();
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 1),
            UserId.New(), activityId, new ActivityDetailsSpecification("test_title", null), null,
            null);
        
        // Act
        var exception = Record.Exception(() => dailyTracker.MarkActivityAsChecked(ActivityId.New()));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.NotExists");
    }
}