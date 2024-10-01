using discipline.application.Behaviours;
using discipline.domain.UsersCalendars.Repositories;
using discipline.domain.UsersCalendars.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using SharpCompress.Archives;

namespace discipline.application.Features.UsersCalendars;

internal static class ChangeEventDate
{
    internal static WebApplication MapChangeEventDate(this WebApplication app)
    {
        return app;
    }
}

public sealed record ChangeEventDateCommand(Guid UserId, Guid EventId, DateOnly NewDate) : ICommand;

public sealed class ChangeEventDateCommandValidator : AbstractValidator<ChangeEventDateCommand>
{
    public ChangeEventDateCommandValidator()
    { 
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("\"UserId\" can not be empty");
        
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("\"EventId\" can not be empty");

        RuleFor(x => x.NewDate)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("\"NewDate\" can not be min date");
    }
}

internal sealed class ChangeEventDateCommandHandler(
    IChangeEventUserCalendarService service) : ICommandHandler<ChangeEventDateCommand>
{
    public async Task HandleAsync(ChangeEventDateCommand command, CancellationToken cancellationToken = default)
        => await service.Invoke(command.UserId, command.EventId, command.NewDate, cancellationToken);
    
}