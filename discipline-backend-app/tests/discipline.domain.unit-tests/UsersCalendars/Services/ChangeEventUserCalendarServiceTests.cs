using discipline.domain.UsersCalendars.Entities;
using discipline.domain.UsersCalendars.Exceptions;
using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services;
using discipline.domain.UsersCalendars.Services.Abstractions;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.UsersCalendars.Services;

public sealed class ChangeEventUserCalendarServiceTests
{
    private Task Act(Guid userId, Guid eventId, DateOnly newDate) => _changeEventUserCalendarService
        .Invoke(userId, eventId, newDate, default);

    [Fact]
    public async Task Invoke_GivenNewDateForNotExistingUserCalendar_ShouldAddUserCalendarForDateWithEventAndRemoveEventForPresentUserCalendar()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var eventId = Guid.NewGuid();
        userCalendar.AddEvent(eventId, "test_event_title");
        var newDate = userCalendar.Day.Value.AddDays(1);

        //act
        await Act(userCalendar.UserId, eventId, newDate);
        
        //assert
        userCalendar.Events.Any(x => x.Id.Value == eventId).ShouldBeFalse();
        await _userCalendarRepository
            .Received(1)
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == newDate 
                && arg.Events.Any(x => x.Id.Value == eventId)));
    }
    
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