using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class DeleteActivityStageCommandHandlerTests
{
    private Task Act(DeleteActivityStageCommand command) => _handler.HandleAsync(command, CancellationToken.None);
    
    [Fact]
    public async Task GivenExistingDailyTracker_WhenHandleAsync_ThenShouldUpdateDailyTracker()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var userId = UserId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, new DateOnly(2025, 1, 1),
            userId, activityId, new ActivityDetailsSpecification("test_title", null), null,
            [new StageSpecification("test_title", 1)]);
        var stage = dailyTracker.Activities.Single().Stages!.Single();

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTrackerId, userId, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new DeleteActivityStageCommand(userId, dailyTrackerId, activityId, stage.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _writeReadDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker);
    }
    
    [Fact]
    public async Task GivenExistingDailyTracker_WhenHandleAsync_ThenShouldRemoveStageFromDailyTracker()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var userId = UserId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, new DateOnly(2025, 1, 1),
            userId, activityId, new ActivityDetailsSpecification("test_title", null), null,
            [new StageSpecification("test_title", 1)]);
        var stage = dailyTracker.Activities.Single().Stages!.Single();

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTrackerId, userId, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new DeleteActivityStageCommand(userId, dailyTrackerId, activityId, stage.Id);
        
        // Act
        await Act(command);
        
        // Assert
        dailyTracker.Activities.Single().Stages!.ShouldBeNull();
    }

    [Fact]
    public async Task GivenNotExistingDailyTracker_WhenHandleAsync_ThenShouldNotUpdateDailyTracker()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var userId = UserId.New();
        var activityId = ActivityId.New();
        var stageId = StageId.New();

        var command = new DeleteActivityStageCommand(userId, dailyTrackerId, activityId, stageId);
        
        // Act
        await Act(command);
        
        // Assert
        await _writeReadDailyTrackerRepository
            .Received(0)
            .UpdateAsync(Arg.Any<DailyTracker>());
    }

    [Fact]
    public async Task GivenNotExistingActivity_WhenHandleAsync_ThenShouldNotUpdateDailyTracker()
    {
        
        var dailyTrackerId = DailyTrackerId.New();
        var userId = UserId.New();
        var activityId = ActivityId.New();

        var dailyTracker = DailyTracker.Create(dailyTrackerId, new DateOnly(2025, 1, 1),
            userId, activityId, new ActivityDetailsSpecification("test_title", null), null,
            [new StageSpecification("test_title", 1)]);

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTrackerId, userId, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new DeleteActivityStageCommand(userId, dailyTrackerId, ActivityId.New(), StageId.New());
        
        // Act
        await Act(command);
        
        // Assert
        await _writeReadDailyTrackerRepository
            .Received(0)
            .UpdateAsync(dailyTracker);
    }
    
    #region Arrange
    private readonly IReadWriteDailyTrackerRepository _writeReadDailyTrackerRepository;
    private readonly DeleteActivityStageCommandHandler _handler;

    public DeleteActivityStageCommandHandlerTests()
    {
        _writeReadDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new DeleteActivityStageCommandHandler(_writeReadDailyTrackerRepository);
    }
    #endregion
}