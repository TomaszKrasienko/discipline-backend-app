using discipline.domain.UsersCalendars.Exceptions;
using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services;
using discipline.domain.UsersCalendars.Services.Abstractions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.UsersCalendars.Services;

public sealed class ChangeEventUserCalendarServiceTests
{
    private Task Act(Guid userId, Guid eventId, DateOnly newDate) => _changeEventUserCalendarService
        .Invoke(userId, eventId, newDate, default);

    
    
    [Fact]
    public async Task Invoke_GivenEventIdForNotExistingUserCalendar_ShouldThrowUserCalendarForEventNotFoundException()
    {
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(Guid.NewGuid(), Guid.NewGuid(), new DateOnly(2024, 1, 1)));
        
        //assert
        exception.ShouldBeOfType<UserCalendarForEventNotFoundException>();
    }
    
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly IChangeEventUserCalendarService _changeEventUserCalendarService;

    public ChangeEventUserCalendarServiceTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _changeEventUserCalendarService = new ChangeEventUserCalendarService(
            _userCalendarRepository);
    }
    #endregion
}