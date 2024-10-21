using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.AddMeeting;

public sealed class AddMeetingCommandHandlerTests
{
    private Task Act(AddMeetingCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUserCalendarForDate_ShouldAddUserCalendarWithMeeting()
    {
        //arrange
        var command = new AddMeetingCommand(new DateOnly(2024, 1, 1),  UserId.New(), 
            EventId.New(), "test_title", new TimeOnly(10, 00), new TimeOnly(11, 00), "test_platform",
            "test_uri", null);
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                && arg.UserId == command.UserId
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                          && x.Title.Value == command.Title
                          && ((Meeting)x).MeetingTimeSpan.From == command.TimeFrom
                          && ((Meeting)x).MeetingTimeSpan.To == command.TimeTo
                          && ((Meeting)x).Address.Platform == command.Platform
                          && ((Meeting)x).Address.Uri == command.Uri
                          && ((Meeting)x).Address.Place == command.Place
                          && x is Meeting)));
        
        await _userCalendarRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserCalendar>());
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingUserCalendarForDate_ShouldUpdateUserCalendarWithMeeting()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var command = new AddMeetingCommand(userCalendar.Day, userCalendar.UserId, EventId.New(), 
            "test_title", new TimeOnly(10, 00), new TimeOnly(11, 00), null,
            null, "place");

        _userCalendarRepository
            .GetForUserByDateAsync(command.UserId, command.Day)
            .Returns(userCalendar);
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .Received(1)
            .UpdateAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                && arg.UserId == command.UserId
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                          && x.Title.Value == command.Title
                          && ((Meeting)x).MeetingTimeSpan.From == command.TimeFrom
                          && ((Meeting)x).MeetingTimeSpan.To == command.TimeTo
                          && ((Meeting)x).Address.Platform == command.Platform
                          && ((Meeting)x).Address.Uri == command.Uri
                          && ((Meeting)x).Address.Place == command.Place
                          && x is Meeting)));
        
        await _userCalendarRepository
            .Received(0)
            .AddAsync(Arg.Any<UserCalendar>());
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<AddMeetingCommand> _handler;

    public AddMeetingCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new AddMeetingCommandHandler(_userCalendarRepository);
    }
    #endregion
}