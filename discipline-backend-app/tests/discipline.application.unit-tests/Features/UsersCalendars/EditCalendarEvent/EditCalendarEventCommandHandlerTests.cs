using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Exceptions;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.EditCalendarEvent;

public sealed class EditCalendarEventCommandHandlerTests
{
    private Task Act(EditCalendarEventCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenExistingUserCalendarForUserAndEventId_ShouldEditAndUpdateByRepository()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_title", new TimeOnly(12, 00), null, 
            "test_action");
        var command = new EditCalendarEventCommand(UserId.New(), eventId, "new_test_title",
            new TimeOnly(13, 00), null, "new_test_action");

        _userCalendarRepository
            .GetByEventIdAsync(command.UserId, command.Id)
            .Returns(userCalendar);
        
        //act
        await Act(command);
        
        //assert
        var @event = userCalendar.Events.First(x => x.Id == command.Id);
        @event.Title.Value.ShouldBe(command.Title);
        ((CalendarEvent)@event).MeetingTimeSpan.From.ShouldBe(command.TimeFrom);
        ((CalendarEvent)@event).MeetingTimeSpan.To.ShouldBe(command.TimeTo);
        ((CalendarEvent)@event).Action.Value.ShouldBe(command.Action);
        
        await _userCalendarRepository
            .UpdateAsync(userCalendar);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUserCalendarForUserAndEventId_ShouldThrowUserCalendarNotFoundException()
    {
        //arrange
        var command = new EditCalendarEventCommand(UserId.New(), EventId.New(), "test_title",
            new TimeOnly(12, 00), null, "test_action");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));
        
        //assert
        exception.ShouldBeOfType<UserCalendarNotFoundException>();
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<EditCalendarEventCommand> _handler;
    
    public EditCalendarEventCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new EditCalendarEventCommandHandler(_userCalendarRepository);
    }
    #endregion
}