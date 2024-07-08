using discipline.application.Behaviours;
using discipline.application.Domain.DailyProductivities.Exceptions;
using discipline.application.Domain.DailyProductivities.Repositories;
using discipline.application.Features.DailyProductivities;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.DailyProductivities.DeleteActivity;

public sealed class DeleteActivityCommandHandlerTests
{
    private Task Act(DeleteActivityCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingDailyProductivityForActivityId_ShouldUpdateDailyProductivityAfterRemovingActivity()
    {
        //arrange
        var dailyProductivity = DailyProductivityFactory.Get();
        var activity = ActivityFactory.GetInDailyProductivity(dailyProductivity);
        var command = new DeleteActivityCommand(activity.Id);

        _dailyProductivityRepository
            .GetByActivityId(command.Id)
            .Returns(dailyProductivity);
        
        //act
        await Act(command);
        
        //assert
        dailyProductivity
            .Activities
            .Any(x => x.Id.Equals(command.Id)).ShouldBeFalse();

        await _dailyProductivityRepository
            .Received(1)
            .UpdateAsync(dailyProductivity);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingDailyProductivityForActivityId_ShouldThrowActivityNotFoundException()
    {
        //arrange
        var command = new DeleteActivityCommand(Guid.NewGuid());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType < ActivityNotFoundException>();
    }
    
    #region arrange
    private readonly IDailyProductivityRepository _dailyProductivityRepository;
    private readonly ICommandHandler<DeleteActivityCommand> _handler;

    public DeleteActivityCommandHandlerTests()
    {
        _dailyProductivityRepository = Substitute.For<IDailyProductivityRepository>();
        _handler = new DeleteActivityCommandHandler(_dailyProductivityRepository);
    }
    #endregion
}