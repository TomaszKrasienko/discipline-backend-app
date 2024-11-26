using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Exceptions;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.EditImportantDate;

public sealed class EditImportantDateCommandHandlerTests
{
    private Task Act(EditImportantDateCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenExistingUserCalendarForUserAndEventId_ShouldEditAndUpdateByRepository()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_title");
        var command = new EditImportantDateCommand(userCalendar.UserId, eventId, "new_test_title");

        _userCalendarRepository
            .GetByEventIdAsync(command.UserId, command.Id)
            .Returns(userCalendar);
        
        //act
        await Act(command);
        
        //assert
        var @event = userCalendar.Events.First(x => x.Id == command.Id);
        @event.Title.Value.ShouldBe(command.Title);
        
        await _userCalendarRepository
            .UpdateAsync(userCalendar);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUserCalendarForUserAndEventId_ShouldThrowUserCalendarNotFoundException()
    {
        //arrange
        var command = new EditImportantDateCommand(UserId.New(), EventId.New(), "test_title");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));
        
        //assert
        exception.ShouldBeOfType<UserCalendarNotFoundException>();
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<EditImportantDateCommand> _handler;

    public EditImportantDateCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new EditImportantDateCommandHandler(_userCalendarRepository);
    }
    #endregion
}