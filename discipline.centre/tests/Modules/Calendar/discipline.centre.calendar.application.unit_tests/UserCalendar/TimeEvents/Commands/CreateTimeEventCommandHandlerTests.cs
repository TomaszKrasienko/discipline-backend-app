using discipline.centre.calendar.application.UserCalendar.TimeEvents.Commands.CreateTimeEvent;
using discipline.centre.calendar.domain;
using discipline.centre.calendar.domain.Repositories;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;

namespace discipline.centre.calendar.application.unit_tests.UserCalendar.TimeEvents.Commands;

public sealed class CreateTimeEventCommandHandlerTests
{
    private Task Act(CreateTimeEventCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenNotExistingCalendarDay_WhenHandleAsync_ThenShouldAddUserCalendarDayWithTimeEvent()
    {
        // Arrange
        var command = new CreateTimeEventCommand(UserId.New(), new DateOnly(2025,1,2), CalendarEventId.New(), "test_title", "test_description",
            new TimeOnly(11, 00), new TimeOnly(12, 00));

        _readWriteUserCalendarRepository
            .GetByDayAsync(command.UserId, command.Day, CancellationToken.None)
            .ReturnsNull();
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteUserCalendarRepository
            .Received(1)
            .AddAsync(Arg.Is<UserCalendarDay>(calendar
                => calendar.Day == command.Day
                && calendar.UserId == command.UserId
                && calendar.Events.Any(@event
                       => @event is TimeEvent
                       && @event.Id == command.EventId)), CancellationToken.None);
        
        await _readWriteUserCalendarRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserCalendarDay>(), CancellationToken.None);
    }

    [Fact]
    public async Task GivenExistingCalendarDay_WhenHandleAsync_ThenShouldUpdateUserCalendarDayWithTimeEvent()
    {
        // Arrange
        var userCalendar = UserCalendarDay.CreateWithImportantDate(UserCalendarId.New(), UserId.New(), new DateOnly(2025,1,1), CalendarEventId.New(), 
            "test_title", null);
        
        var command = new CreateTimeEventCommand(userCalendar.UserId, userCalendar.Day, CalendarEventId.New(), "test_time_event_title", "description",
            new TimeOnly(11, 00), new TimeOnly(12, 00));
        
        _readWriteUserCalendarRepository
            .GetByDayAsync(command.UserId, command.Day, CancellationToken.None)
            .Returns(userCalendar);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteUserCalendarRepository
            .UpdateAsync(userCalendar, CancellationToken.None);

        userCalendar.Events.Any(@event => @event is TimeEvent && @event.Id == command.EventId).ShouldBeTrue();
    }
    
    #region Arrange
    private readonly IReadWriteUserCalendarRepository  _readWriteUserCalendarRepository;
    private readonly CreateTimeEventCommandHandler _handler;

    public CreateTimeEventCommandHandlerTests()
    {
        _readWriteUserCalendarRepository = Substitute.For<IReadWriteUserCalendarRepository>();
        _handler = new CreateTimeEventCommandHandler(_readWriteUserCalendarRepository);
    }

    #endregion
}