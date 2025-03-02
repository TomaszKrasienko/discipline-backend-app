using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unit_tests.DailyTrackerTests;

public sealed class ClearParentActivityRuleIdsTests
{
    [Fact]
    public void GivenDailyTrackerWithActivitiesFromActivityRule_WhenClearParentActivityRuleIdIsCalled_ThenShouldChangeParentActivityRuleIdToNullOnAllActivities()
    {
        // Arrange
        var parentActivityRuleId = ActivityRuleId.New();
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 1),
            UserId.New(), ActivityId.New(), new ActivityDetailsSpecification("test", null),
            parentActivityRuleId, []);
        
        dailyTracker.AddActivity(ActivityId.New(), new ActivityDetailsSpecification("test", null),
            parentActivityRuleId, []);
    
        // Act
        dailyTracker.ClearParentActivityRuleIdIs(parentActivityRuleId);
        
        // assert
        dailyTracker.Activities.Any(x => x.ParentActivityRuleId == parentActivityRuleId).ShouldBeFalse();
    }
}