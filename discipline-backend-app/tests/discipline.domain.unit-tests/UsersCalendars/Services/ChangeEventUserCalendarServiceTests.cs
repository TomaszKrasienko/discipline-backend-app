using discipline.domain.SharedKernel.TypeIdentifiers;
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
    private Task Act(UserId userId, EventId eventId, DateOnly newDate) => _changeEventUserCalendarService
        .Invoke(userId, eventId, newDate, default);

    [Fact]
    public async Task Invoke_GivenNewDateForNotExistingUserCalendar_ShouldAddUserCalendarForDateWithEventAndRemoveEventAndUpdateForPresentUserCalendar()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_event_title");
        _userCalendarRepository
            .GetByEventIdAsync(userCalendar.UserId, eventId, default)
            .Returns(userCalendar);
        
        var newDate = userCalendar.Day.Value.AddDays(1);

        //act
        await Act(userCalendar.UserId, eventId, newDate);
        
        //assert
        userCalendar.Events.Any(x => x.Id == eventId).ShouldBeFalse();
        await _userCalendarRepository
            .Received(1)
            .AddAsync(Arg.Is<UserCalendar>(arg
                => arg.Day.Value == newDate 
                && arg.Events.Any(x => x.Id == eventId)));
        await _userCalendarRepository
            .Received(1)
            .UpdateAsync(userCalendar);
    }
    
    [Fact]
    public async Task Invoke_GivenNewDateForNotExistingUserCalendar_ShouldAddEventToUserCalendarAndRemoveEventForPresentUserCalendar()
    {
        //arrange
        var userCalendar = UserCalendarFactory.Get();
        var eventId = EventId.New();
        userCalendar.AddEvent(eventId, "test_event_title");
        _userCalendarRepository
            .GetByEventIdAsync(userCalendar.UserId, eventId, default)
            .Returns(userCalendar);
        var newUserCalendar = UserCalendar.Create(UserCalendarId.New(), userCalendar.Day.Value.AddDays(2), userCalendar.UserId);
        _userCalendarRepository
            .GetForUserByDateAsync(userCalendar.UserId, userCalendar.Day)
            .Returns(newUserCalendar);
    
        //act
        await Act(userCalendar.UserId, eventId, userCalendar.Day);
        
        //assert
        userCalendar.Events.Any(x => x.Id == eventId).ShouldBeFalse();
        newUserCalendar.Events.Any(x => x.Id == eventId).ShouldBeTrue();
        await _userCalendarRepository
            .Received(1)
            .UpdateAsync(newUserCalendar);
    }
    
    [Fact]
    public async Task Invoke_GivenEventIdForNotExistingUserCalendar_ShouldThrowUserCalendarForEventNotFoundException()
    {
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(UserId.New(), EventId.New(), new DateOnly(2024, 1, 1)));
        
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