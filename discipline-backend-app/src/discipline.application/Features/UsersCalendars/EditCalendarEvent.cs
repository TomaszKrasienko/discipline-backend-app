using discipline.application.Behaviours;
using discipline.domain.UsersCalendars.Repositories;

namespace discipline.application.Features.UsersCalendars;

internal sealed class EditCalendarEvent
{
    
}

public sealed record EditCalendarEventCommand(Guid UserId, Guid Id, string Title, TimeOnly TimeFrom,
    TimeOnly? TimeTo, string Action) : ICommand;

internal sealed class EditCalendarEventCommandHandler(
    IUserCalendarRepository userCalendarRepository) : ICommandHandler<EditCalendarEventCommand>
{
    public Task HandleAsync(EditCalendarEventCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}