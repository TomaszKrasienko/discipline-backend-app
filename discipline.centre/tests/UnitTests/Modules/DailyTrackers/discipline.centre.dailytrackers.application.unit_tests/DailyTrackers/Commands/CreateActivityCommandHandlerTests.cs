using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.dailytrackers.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class CreateActivityCommandHandlerTests
{
    private Task Act(CreateActivityCommand command) => _handler.HandleAsync(command);

    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndNotExistingDailyTracker_ShouldAddDailyTracker()
    {
        //arrange
        var command = new CreateActivityCommand(UserId.New(), ActivityId.New(), new DateOnly(2025,1,1),
        new ActivityDetailsSpecification("new_test_activity", null),
        null);

        _writeReadDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.Day, command.UserId, default)
            .ReturnsNull();

        //act
        await Act(command);
        
        //assert
        await _writeReadDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<DailyTracker>(arg
                => arg.Day == command.Day
                   && arg.UserId == command.UserId
                   && arg.Activities.Count == 1
                   && arg.Activities.Any(x
                       => x.Id == command.ActivityId 
                       && x.Details.Title == command.Details.Title)
                   ), default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndExistingDailyTracker_ShouldAddActivityToDailyTracker()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.UserId, ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification("new_test_activity", null),
            null);
        
        _writeReadDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.Day, dailyTracker.UserId)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        dailyTracker.Activities
            .Any(x => x.Id == command.ActivityId).ShouldBeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndExistingDailyTracker_ShouldUpdateDailyTracker()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.UserId, ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification("new_test_activity", null),
            null);
        
        _writeReadDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.Day, dailyTracker.UserId)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        await _writeReadDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, default);
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingActivityTitleForDay_ShouldThrowDomainException()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.UserId, ActivityId.New(), dailyTracker.Day,
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