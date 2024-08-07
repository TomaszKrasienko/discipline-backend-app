using discipline.application.Behaviours;
using discipline.application.Domain.ActivityRules;
using discipline.application.Domain.ActivityRules.Entities;
using discipline.application.Domain.ActivityRules.Repositories;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

public static class CreateActivityRule
{
    public static WebApplication MapCreateActivityRule(this WebApplication app)
    {
        app.MapPost($"/{Extensions.ActivityRulesTag}/create", async (CreateActivityRuleCommand command, HttpContext httpContext, 
                    ICommandDispatcher dispatcher, CancellationToken cancellationToken) => 
            {
                var activityRuleId = Guid.NewGuid();
                await dispatcher.HandleAsync(command with { Id = activityRuleId }, cancellationToken);
                httpContext.AddResourceIdHeader(activityRuleId);
                return Results.CreatedAtRoute(nameof(GetActivityRuleById), new {activityRuleId = activityRuleId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(CreateActivityRule))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            });
        return app;
    }
}

public sealed record CreateActivityRuleCommand(Guid Id, string Title, string Mode,
    List<int> SelectedDays) : ICommand;

public sealed class CreateActivityRuleCommandValidator : AbstractValidator<CreateActivityRuleCommand>
{
    public CreateActivityRuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Activity rule \"ID\" can not be empty");
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Title\" can not be null or empty");
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Activity rule \"Title\" has invalid length");
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class CreateActivityRuleCommandHandler(
    IActivityRuleRepository activityRuleRepository) : ICommandHandler<CreateActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var isExists = await activityRuleRepository.ExistsAsync(command.Title, cancellationToken);
        if (isExists)
        {
            throw new ActivityRuleTitleAlreadyRegisteredException(command.Title);
        }

        var activity = ActivityRule.Create(command.Id, command.Title, command.Mode, command.SelectedDays);
        await activityRuleRepository.AddAsync(activity, cancellationToken);
    }
}
