using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.domain.ActivityRules.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

public static class EditActivityRule
{
    public static WebApplication MapEditActivityRule(this WebApplication app)
    {
        app.MapPut($"/{Extensions.ActivityRulesTag}/{{activityRuleId:guid}}/edit", async (Guid activityRuleId, EditActivityRuleCommand command, HttpContext httpContext, 
                    ICommandDispatcher dispatcher, CancellationToken cancellationToken) 
                => {
                        await dispatcher.HandleAsync(command with { Id = activityRuleId }, cancellationToken);
                        return Results.Ok();
                })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(EditActivityRule))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Updates activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}

public sealed record EditActivityRuleCommand(Guid Id, string Title, string Mode, 
    List<int> SelectedDays) : ICommand;

public sealed class EditActivityRuleCommandValidator : AbstractValidator<EditActivityRuleCommand>
{
    public EditActivityRuleCommandValidator()
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
            .MaximumLength(40)
            .WithMessage("Activity rule \"Title\" has invalid length");
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class EditActivityRuleCommandHandler(
    IActivityRuleRepository activityRuleRepository) : ICommandHandler<EditActivityRuleCommand>
{
    public async Task HandleAsync(EditActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await activityRuleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (activityRule is null)
        {
            throw new ActivityRuleNotFoundException(command.Id);
        }
        
        activityRule.Edit(command.Title, command.Mode, command.SelectedDays);
        await activityRuleRepository.UpdateAsync(activityRule, cancellationToken);
    }
}