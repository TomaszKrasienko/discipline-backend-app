using discipline.application.Behaviours;
using discipline.application.Features.DailyProductivities.Configuration;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.DailyProductivities;

internal static class CreateActivity
{
    internal static WebApplication MapCreateActivity(this WebApplication app)
    {
        app.MapPost($"/{Extensions.DailyProductivityTag}/{{day:datetime}}/add-activity", async (DateTime day, CreateActivityCommand command,
            CancellationToken cancellationToken, ICommandDispatcher commandDispatcher, IIdentityContext identityContext) =>
            {
                var activityId = Guid.NewGuid();
                await commandDispatcher.HandleAsync(command with
                {
                    Id = activityId,
                    Day = DateOnly.FromDateTime(day),
                    UserId = identityContext.UserId
                }, cancellationToken);
                return Results.Ok();
            })            
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(ErrorDto))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(CreateActivity))
            .WithTags(Extensions.DailyProductivityTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            });
        return app;
    }
}

public sealed record CreateActivityCommand(Guid Id, Guid UserId, string Title, DateOnly Day) : ICommand;

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

        RuleFor(x => x.Day)
            .NotEqual(DateOnly.FromDateTime(default))
            .WithMessage("Date can not be empty");
    }
}

internal sealed class CreateActivityCommandHandler(
    IDailyProductivityRepository dailyProductivityRepository) : ICommandHandler<CreateActivityCommand>
{
    public async Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken = default)
    {
        var dailyProductivity = await dailyProductivityRepository.GetByDateAsync(command.Day, cancellationToken);
        if (dailyProductivity is null)
        {
            dailyProductivity = DailyProductivity.Create(command.Day, command.UserId);
            dailyProductivity.AddActivity(command.Id, command.Title);
            await dailyProductivityRepository.AddAsync(dailyProductivity, cancellationToken);
            return;
        }
        
        dailyProductivity.AddActivity(command.Id, command.Title);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}