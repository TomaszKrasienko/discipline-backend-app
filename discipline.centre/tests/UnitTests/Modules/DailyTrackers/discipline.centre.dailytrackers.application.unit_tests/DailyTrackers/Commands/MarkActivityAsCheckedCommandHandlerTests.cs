using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class MarkActivityAsCheckedCommandHandlerTests
{
    private Task Act(MarkActivityAsCheckedCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenExistingDailyTrackerAndActivity_WhenHandleAsync_ThenShouldUpdateDailyTracker()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.Id, dailyTracker.UserId, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new MarkActivityAsCheckedCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _writeReadDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingDailyTrackerAndActivity_WhenHandleAsync_ThenShouldMarkActivityAsChecked()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.Id, dailyTracker.UserId, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new MarkActivityAsCheckedCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        dailyTracker.Activities.Single().IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenNotExistingDailyTracker_WhenHandleAsync_ThenThrowNotFoundException()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
        var userId = UserId.New();
        var command = new MarkActivityAsCheckedCommand(userId, dailyTrackerId, activityId);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    #region arrange
    private readonly IReadWriteDailyTrackerRepository _writeReadDailyTrackerRepository;
    private readonly MarkActivityAsCheckedCommandHandler _handler;

    public MarkActivityAsCheckedCommandHandlerTests()
    {
        _writeReadDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new MarkActivityAsCheckedCommandHandler(_writeReadDailyTrackerRepository);
    }

    #endregion
}