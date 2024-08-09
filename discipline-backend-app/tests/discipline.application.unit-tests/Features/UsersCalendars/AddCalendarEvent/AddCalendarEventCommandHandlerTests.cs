using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars;
using discipline.tests.shared.Entities;
using NSubstitute;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.AddCalendarEvent;

public sealed class AddCalendarEventCommandHandlerTests
{
    private Task Act(AddCalendarEventCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingUserCalendarForDate_ShouldAddUserCalendarWithEventCalendar()
    {
        //arrange
        var command = new AddCalendarEventCommand(new DateOnly(2024, 1, 1), Guid.NewGuid(),
            "test_title", new TimeOnly(10, 00), new TimeOnly(11, 00), "test_action");
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                       && x.Title.Value == command.Title
                       && x is CalendarEvent)));
        
        await _userCalendarRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserCalendar>());
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingUserCalendarForDate_ShouldUpdateUserCalendarWithCalendarEvent()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var command = new AddCalendarEventCommand(userCalendar.Day, Guid.NewGuid(),
            "test_title", new TimeOnly(10, 00), new TimeOnly(11, 00), "test_action");

        _userCalendarRepository
            .GetByDateAsync(command.Day)
            .Returns(userCalendar);
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .Received(1)
            .UpdateAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                       && x.Title.Value == command.Title
                       && x is CalendarEvent)));
        
        await _userCalendarRepository
            .Received(0)
            .AddAsync(Arg.Any<UserCalendar>());
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<AddCalendarEventCommand> _handler;
    
    public AddCalendarEventCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new AddCalendarEventCommandHandler(_userCalendarRepository);
    }
    #endregion
}