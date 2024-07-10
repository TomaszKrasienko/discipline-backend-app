using discipline.application.Behaviours;
using discipline.application.Domain.UsersCalendars.Entities;
using discipline.application.Domain.UsersCalendars.Repositories;
using discipline.application.Features.UsersCalendars;
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
        var command = new AddImportantDateCommand(new DateOnly(2024, 1, 1), Guid.NewGuid(),
            "test_title");
        
        //act
        await Act(command);
        
        //assert
        await _userCalendarRepository
            .Received(1)
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == command.Day
                   && arg.Events.Any(x
                       => x.Id.Equals(command.Id)
                          && x.Title.Value == command.Title)));
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