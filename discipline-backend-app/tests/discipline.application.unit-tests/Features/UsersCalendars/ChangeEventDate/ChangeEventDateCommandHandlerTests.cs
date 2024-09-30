using discipline.application.Behaviours;
using discipline.application.Features.UsersCalendars;
using discipline.domain.UsersCalendars.Repositories;
using NSubstitute;

namespace discipline.application.unit_tests.Features.UsersCalendars.ChangeEventDate;

public sealed class ChangeEventDateCommandHandlerTests
{
    #region arrange
    private readonly IUserCalendarRepository _userCalendarRepository;
    private readonly ICommandHandler<ChangeEventDateCommand> _handler;

    public ChangeEventDateCommandHandlerTests()
    {
        _userCalendarRepository = Substitute.For<IUserCalendarRepository>();
        _handler = new ChangeEventDateCommandHandler(_userCalendarRepository);
    }
    #endregion
}