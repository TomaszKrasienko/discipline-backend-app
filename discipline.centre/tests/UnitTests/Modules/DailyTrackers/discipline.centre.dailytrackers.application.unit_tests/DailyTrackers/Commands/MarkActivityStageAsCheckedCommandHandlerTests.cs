using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class MarkActivityStageAsCheckedCommandHandlerTests
{
    private Task Act(MarkActivityStageAsCheckedCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task HandleAsync_ShouldChangeStageIsCheckedAsTrue_WhenDailyTrackerExists()
    {
        //arrange
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.Now),
            UserId.New(), ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        var activity = dailyTracker.Activities.Single();
        var stage = activity.Stages!.Single();
        
        var command = new MarkActivityStageAsCheckedCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id, stage.Id);

        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(command.UserId, command.DailyTrackerId, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_ShouldUpdateDailyTracker_WhenDailyTrackerExists()
    {
        //arrange
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), DateOnly.FromDateTime(DateTime.Now),
            UserId.New(), ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, [new StageSpecification("test_stage_title", 1)]);
        var activity = dailyTracker.Activities.Single();
        var stage = activity.Stages!.Single();
        
        var command = new MarkActivityStageAsCheckedCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id, stage.Id);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(command.UserId, command.DailyTrackerId, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldThrowNotFoundException_WhenDailyTrackerDoesNotExist()
    {
        //arrange
        var command = new MarkActivityStageAsCheckedCommand(UserId.New(), DailyTrackerId.New(), ActivityId.New(),
            StageId.New());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }

    #region arrange
    private readonly IReadWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly MarkActivityStageAsCheckedCommandHandler _handler;

    public MarkActivityStageAsCheckedCommandHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new MarkActivityStageAsCheckedCommandHandler(_readWriteDailyTrackerRepository);
    }
    #endregion
}