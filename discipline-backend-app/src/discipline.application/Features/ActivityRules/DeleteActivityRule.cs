using discipline.application.Behaviours;
using discipline.application.Behaviours.CQRS;
using discipline.application.Behaviours.CQRS.Commands;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

internal static class DeleteActivityRule
{
    internal static WebApplication MapDeleteActivityRule(this WebApplication app)
    {
        app.MapDelete($"/{Extensions.ActivityRulesTag}/{{activityRuleId}}/delete", async (Ulid activityRuleId,
                CancellationToken cancellationToken, ICqrsDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new DeleteActivityRuleCommand(new ActivityRuleId(activityRuleId)), cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName(nameof(DeleteActivityRule))
            .WithTags(Extensions.ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Deletes activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}

public sealed record DeleteActivityRuleCommand(ActivityRuleId Id) : ICommand;

public sealed class DeleteActivityRuleCommandValidator : AbstractValidator<DeleteActivityRuleCommand>
{
    public DeleteActivityRuleCommandValidator()
    {
        
    }
}

internal sealed class DeleteActivityRuleCommandHandler(
    IActivityRuleRepository activityRuleRepository) : ICommandHandler<DeleteActivityRuleCommand>
{
    public async Task HandleAsync(DeleteActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await activityRuleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (activityRule is null)
        {
            throw new ActivityRuleNotFoundException(command.Id);
        }

        await activityRuleRepository.DeleteAsync(activityRule, cancellationToken);
    }
}