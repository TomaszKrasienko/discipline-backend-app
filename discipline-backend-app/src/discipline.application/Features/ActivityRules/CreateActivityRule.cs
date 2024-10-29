using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.domain.ActivityRules.Entities;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

public static class CreateActivityRule
{
    public static WebApplication MapCreateActivityRule(this WebApplication app)
    {
        app.MapPost($"/{Extensions.ActivityRulesTag}/create", async (CreateActivityRuleCommand command, HttpContextAccessor httpContext, 
                    ICqrsDispatcher dispatcher, CancellationToken cancellationToken, IIdentityContext identityContext) => 
            {
                var activityRuleId = ActivityRuleId.New();
                var userId = identityContext.UserId;
                await dispatcher.HandleAsync(command with { Id = activityRuleId, UserId = userId }, cancellationToken);
                httpContext.AddResourceIdHeader(activityRuleId.ToString());
                return Results.CreatedAtRoute(nameof(GetActivityRuleById), new {activityRuleId = activityRuleId}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
            .WithName(nameof(CreateActivityRule))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);
        return app;
    }
}

public sealed record CreateActivityRuleCommand(ActivityRuleId Id, UserId UserId, string Title, string Mode,
    List<int> SelectedDays) : ICommand;

public sealed class CreateActivityRuleCommandValidator : AbstractValidator<CreateActivityRuleCommand>
{
    public CreateActivityRuleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(id => id != new UserId(Ulid.Empty))
            .WithMessage("Activity rule \"UserId\" can not be empty");
        RuleFor(x => x.Id)
            .Must(id => id != new ActivityRuleId(Ulid.Empty))
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

        var activity = ActivityRule.Create(command.Id, command.UserId, command.Title, 
            command.Mode, command.SelectedDays);
        await activityRuleRepository.AddAsync(activity, cancellationToken);
    }
}