using discipline.centre.dailytrackers.application.DailyTrackers.Events;
using discipline.centre.dailytrackers.application.DailyTrackers.Events.Handler;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Events;

public sealed class ActivityRuleDeletedHandlerTests
{
    private Task Act(ActivityRuleDeleted @event) => _handler.HandleAsync(@event, CancellationToken.None);

    [Fact]
    public async Task GivenExistingActivityWithParentActivityRuleIdForUser_WhenHandleAsync_ThenShouldSetNullOnDailyTrackersActivities()
    {
        // Arrange
        var userId = UserId.New();
        var activityRuleId = ActivityRuleId.New();
        var @event = new ActivityRuleDeleted(userId.Value, activityRuleId.Value);
        var dailyTracker1 = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 1), userId,
            ActivityId.New(), new ActivityDetailsSpecification("test_title", null), activityRuleId, []);
        
        var dailyTracker2 = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 2), userId,
            ActivityId.New(), new ActivityDetailsSpecification("test_title", null), activityRuleId, []);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackersByParentActivityRuleId(userId, activityRuleId, CancellationToken.None)
            .Returns([dailyTracker1, dailyTracker2]);
        
        // Act
        await Act(@event);
        
        // Assert
        dailyTracker1
            .Activities.Any(x => x.ParentActivityRuleId == activityRuleId).ShouldBeFalse();
        
        dailyTracker2
            .Activities.Any(x => x.ParentActivityRuleId == activityRuleId).ShouldBeFalse();
    }
    
    [Fact]
    public async Task GivenExistingActivityWithParentActivityRuleIdForUser_WhenHandleAsync_ThenShouldUpdateGivenDailyTrackers()
    {
        // Arrange
        var userId = UserId.New();
        var activityRuleId = ActivityRuleId.New();
        var @event = new ActivityRuleDeleted(userId.Value, activityRuleId.Value);
        var dailyTracker1 = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 1), userId,
            ActivityId.New(), new ActivityDetailsSpecification("test_title", null), activityRuleId, []);
        
        var dailyTracker2 = DailyTracker.Create(DailyTrackerId.New(), new DateOnly(2025, 1, 2), userId,
            ActivityId.New(), new ActivityDetailsSpecification("test_title", null), activityRuleId, []);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackersByParentActivityRuleId(userId, activityRuleId, CancellationToken.None)
            .Returns([dailyTracker1, dailyTracker2]);
        
        // Act
        await Act(@event);
        
        // Assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateRangeAsync(Arg.Is<IEnumerable<DailyTracker>>(arg => arg.Any(x => x == dailyTracker1)),
                Arg.Any<CancellationToken>());
        
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateRangeAsync(Arg.Is<IEnumerable<DailyTracker>>(arg => arg.Any(x => x == dailyTracker2)), 
                Arg.Any<CancellationToken>());
    }
    
    private readonly IReadWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly ActivityRuleDeletedHandler _handler;

    public ActivityRuleDeletedHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new ActivityRuleDeletedHandler(_readWriteDailyTrackerRepository);
    }
}