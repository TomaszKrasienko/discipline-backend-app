using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class CheckActivityStageCommandHandlerTests
{
    private Task Act(CheckActivityStageCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDailyTrackerNotFound()
    {
        //arrange
        var command = new CheckActivityStageCommand(DailyTrackerId.New(), ActivityId.New(),
            StageId.New(), UserId.New());
        
        //act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    #region arrange
    private readonly IWriteReadDailyTrackerRepository _writeReadDailyTrackerRepository;
    private readonly CheckActivityStageCommandHandler _handler;

    public CheckActivityStageCommandHandlerTests()
    {
        _writeReadDailyTrackerRepository = Substitute.For<IWriteReadDailyTrackerRepository>();
        _handler = new CheckActivityStageCommandHandler(_writeReadDailyTrackerRepository);
    }
    #endregion
}