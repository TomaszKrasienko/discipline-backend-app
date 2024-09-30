using discipline.application.Behaviours;
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

public sealed record ChangeEventDateCommand(Guid EventId, DateOnly NewDate) : ICommand;

public sealed class ChangeEventDateCommandValidator : AbstractValidator<ChangeEventDateCommand>
{
    public ChangeEventDateCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("\"EventId\" can not be empty");

        RuleFor(x => x.NewDate)
            .NotEqual(DateOnly.MinValue)
            .WithMessage("\"NewDate\" can not be min date");
    }
}

internal sealed class ChangeEventDateCommandHandler : ICommandHandler<ChangeEventDateCommand>
{
    public Task HandleAsync(ChangeEventDateCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}