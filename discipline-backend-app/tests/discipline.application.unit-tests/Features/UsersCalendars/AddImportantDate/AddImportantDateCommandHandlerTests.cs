using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Repositories;
using discipline.tests.shared.Entities;
using NSubstitute;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.AddImportantDate;

public sealed class AddImportantDateCommandHandlerTests
{
    private Task Act(AddImportantDateCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenNotExistingUserCalendarForDate_ShouldAddNewUserCalendarWithImportantDate()
    {
        //arrange
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), UserId.New(), 
            EventId.New(), "test_title");
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .Received(1)
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                && arg.UserId == command.UserId
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                          && x.Title.Value == command.Title)));
        await _userCalendarRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserCalendar>());
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingUserCalendarForDate_ShouldUpdateUserCalendarWithImportantDate()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var command = new AddImportantDateCommand(userCalendar.Day, userCalendar.UserId, EventId.New(), 
            "test_title");

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
                          && x.Title.Value == command.Title)));
        
        await _userCalendarRepository
            .Received(0)
            .AddAsync(Arg.Any<UserCalendar>());
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<AddImportantDateCommand> _handler;

    public AddImportantDateCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new AddImportantDateCommandHandler(_userCalendarRepository);
    }
    #endregion
}