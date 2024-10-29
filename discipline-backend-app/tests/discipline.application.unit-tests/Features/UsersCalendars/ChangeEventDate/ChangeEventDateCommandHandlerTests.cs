using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Features.UsersCalendars;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.UsersCalendars.Services.Abstractions;
using NSubstitute;
using Xunit;

namespace discipline.application.unit_tests.Features.UsersCalendars.ChangeEventDate;

public sealed class ChangeEventDateCommandHandlerTests
{
    private Task Act(ChangeEventDateCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task Handle_GivenCommand_ShouldInvokeChangeEventUserCalendarService()
    {
        //arrange
        var command = new ChangeEventDateCommand(UserId.New(), EventId.New(), new DateOnly(2024, 1, 1));
        
        //act
        await Act(command);
        
        //assert
        await _service
            .Received(1)
            .Invoke(command.UserId, command.EventId, command.NewDate, default);
    }
    
    #region arrange
    private readonly IChangeEventUserCalendarService _service;
    private readonly ICommandHandler<ChangeEventDateCommand> _handler;

    public ChangeEventDateCommandHandlerTests()
    {
        _service = Substitute.For<IChangeEventUserCalendarService>();
        _handler = new ChangeEventDateCommandHandler(_service);
    }
    #endregion
}