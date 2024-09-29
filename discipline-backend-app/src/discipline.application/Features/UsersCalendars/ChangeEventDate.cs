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
        
    }
}