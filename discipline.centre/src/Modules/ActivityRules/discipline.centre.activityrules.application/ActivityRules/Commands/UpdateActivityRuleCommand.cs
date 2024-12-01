using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record UpdateActivityRuleCommand(ActivityRuleId Id, UserId UserId, string Title, string? Note, 
    string Mode, List<int>? SelectedDays) : ICommand;

public sealed class UpdateActivityRuleCommandValidator : AbstractValidator<UpdateActivityRuleCommand>
{
    public UpdateActivityRuleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Title\" can not be null or empty");
        RuleFor(x => x.Title)
            .MaximumLength(30)
            .WithMessage("Activity rule \"Title\" has invalid length");
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class UpdateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<UpdateActivityRuleCommand>
{
    public async Task HandleAsync(UpdateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await readWriteActivityRuleRepository.GetByIdAsync(command.Id, command.UserId, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("UpdateActivityRule.ActivityRule", nameof(activityRule), command.Id.ToString());
        }

        if (activityRule.HasChanges(command.Title, command.Note, command.Mode, command.SelectedDays))
        {
            activityRule.Edit(command.Title, command.Note, command.Mode, command.SelectedDays);
            await readWriteActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
        }
    }
}