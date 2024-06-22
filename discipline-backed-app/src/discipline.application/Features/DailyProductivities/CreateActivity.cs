using discipline.application.Behaviours;
using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Features.Configuration.Base.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.DailyProductivities;

internal static class CreateActivity
{
    internal static WebApplication MapCreateActivity(this WebApplication app)
    {
        app.MapPost("/daily-productive/current/add-activity", async (CreateActivityCommand command,
            HttpContext httpContext, CancellationToken cancellationToken, ICommandDispatcher commandDispatcher) =>
            {
                var activityId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command with { Id = activityId }, cancellationToken);
                return Results.Ok();
            })            
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(CreateActivity))
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            });
        return app;
    }
}

public sealed record CreateActivityCommand(Guid Id, string Title) : ICommand;

public sealed class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Activity \"ID\" can not be empty");

        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity \"Title\" can not be null or empty");

        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Activity \"Title\" has invalid length");
    }
}

internal sealed class CreateActivityCommandHandler(
    IDailyProductivityRepository dailyProductivityRepository,
    IClock clock) : ICommandHandler<CreateActivityCommand>
{
    public async Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken = default)
    {
        var now = clock.DateNow();
        var dailyProductivity = await dailyProductivityRepository.GetByDateAsync(now, cancellationToken);
        if (dailyProductivity is null)
        {
            dailyProductivity = DailyProductivity.Create(now);
            dailyProductivity.AddActivity(command.Id, command.Title);
            await dailyProductivityRepository.AddAsync(dailyProductivity, cancellationToken);
            return;
        }
        
        dailyProductivity.AddActivity(command.Id, command.Title);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}