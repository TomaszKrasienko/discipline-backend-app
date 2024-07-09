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
    
}