using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.ActivityRules.Configuration;
using discipline.domain.ActivityRules.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace discipline.application.Features.ActivityRules;

internal static class DeleteActivityRule
{
    internal static WebApplication MapDeleteActivityRule(this WebApplication app)
    {
        app.MapDelete($"/{Extensions.ActivityRulesTag}/{{activityRuleId:guid}}/delete", async (Guid activityRuleId,
                CancellationToken cancellationToken, ICommandDispatcher commandDispatcher) =>
            {
                await commandDispatcher.HandleAsync(new DeleteActivityRuleCommand(activityRuleId), cancellationToken);
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(void))
            .WithName(nameof(DeleteActivityRule))
            .WithOpenApi(operation => new (operation)
            {
                Description = "Deletes activity rule"
            });
        return app;
    }
}

public sealed record DeleteActivityRuleCommand(Guid Id) : ICommand;

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