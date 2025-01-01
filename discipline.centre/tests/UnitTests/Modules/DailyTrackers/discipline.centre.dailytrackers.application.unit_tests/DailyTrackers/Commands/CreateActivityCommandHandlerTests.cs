using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class CreateActivityCommandHandlerTests
{
    private Task Act(CreateActivityCommand command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task HandleAsync_GivenExistingActivityTitleForDay_ShouldThrowDomainException()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(UserId.New(), ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification(activity.Details.Title, null),
            null);
        
        _writeReadDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.Day, dailyTracker.UserId)
            .Returns(dailyTracker);
        
        //act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
    
    #region arrange
    private readonly IWriteReadDailyTrackerRepository _writeReadDailyTrackerRepository;
    private readonly ICommandHandler<CreateActivityCommand> _handler;

    public CreateActivityCommandHandlerTests()
    {
        _writeReadDailyTrackerRepository = Substitute.For<IWriteReadDailyTrackerRepository>();
        _handler = new CreateActivityCommandHandler(_writeReadDailyTrackerRepository);
    }
    #endregion
}