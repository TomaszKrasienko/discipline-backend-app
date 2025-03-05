using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class DeleteActivityCommandHandlerTests
{
    private Task Act(DeleteActivityCommand command) => _handler.HandleAsync(command, CancellationToken.None);
    
    [Fact]
    public async Task GivenExistingDailyTrackerWithMoreThanOneActivity_WhenHandleAsyncIsCalled_ShouldUpdateDailyTracker()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        dailyTracker.AddActivity(activity.Id, new ActivityDetailsSpecification(activity.Details.Title, null),
            null, []);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.UserId, dailyTracker.Id, CancellationToken.None)
            .Returns(dailyTracker);
        
        var command = new DeleteActivityCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingDailyTrackerWithMoreThanOneActivity_WhenHandleAsyncIsCalled_ShouldRemoveActivityFromDailyTracker()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        dailyTracker.AddActivity(activity.Id, new ActivityDetailsSpecification(activity.Details.Title, null),
            null, []);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.UserId, dailyTracker.Id, CancellationToken.None)
            .Returns(dailyTracker);
        
        var command = new DeleteActivityCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        dailyTracker
            .Activities.Any(x => x.Id == activity.Id).ShouldBeFalse();
    }

    [Fact]
    public async Task GivenExistingDailyTrackerWithOneActivity_WhenHandleAsyncIsCalled_ShouldRemoveDailyTracker()
    {
        // Arrange
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        var activity = dailyTracker.Activities.Single();
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.UserId, dailyTracker.Id, CancellationToken.None)
            .Returns(dailyTracker);
        
        var command = new DeleteActivityCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .DeleteAsync(dailyTracker, CancellationToken.None);
    }
    
    private readonly IReadWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly DeleteActivityCommandHandler _handler;

    public DeleteActivityCommandHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new DeleteActivityCommandHandler(_readWriteDailyTrackerRepository);
    }
}