using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services;
using discipline.domain.UsersCalendars.Services.Abstractions;
using NSubstitute;

namespace discipline.domain.unit_tests.UsersCalendars.Services;

public sealed class ChangeEventUserCalendarServiceTests
{
    private Task Act(Guid eventId, DateOnly newDate) => _changeEventUserCalendarService
        .Invoke(eventId, newDate);
    
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