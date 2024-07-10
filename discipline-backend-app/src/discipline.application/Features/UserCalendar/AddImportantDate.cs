using discipline.application.Behaviours;
using FluentValidation;
using Microsoft.AspNetCore.Builder;

namespace discipline.application.Features.UserCalendar;

internal static class AddImportantDate
{
    internal static WebApplication MapAddImportantDate(this WebApplication app)
    {
        app.MapPost("user-calendar/add-important-date", () =>
        {

        });
        return app;
    }
}

public sealed record AddImportantDateCommand(DateOnly Day, Guid Id, string Title) : ICommand;

public sealed class AddImportantDateCommandValidator : AbstractValidator<AddImportantDateCommand>
{
    public AddImportantDateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Important date \"ID\" can not be empty");

        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Important date \"Title\" can not be null or empty");

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Important date \"Title\" has invalid length");
    }
}

internal sealed class AddImportantDateCommandHandler : ICommandHandler<AddImportantDateCommand>
{
    public Task HandleAsync(AddImportantDateCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}