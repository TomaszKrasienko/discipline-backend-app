using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record UpdateActivityRuleCommand(ActivityRuleId Id, string Title, string Mode, 
    List<int>? SelectedDays) : ICommand;

public sealed class UpdateActivityRuleCommandValidator : AbstractValidator<UpdateActivityRuleCommand>
{
    public UpdateActivityRuleCommandValidator()
    {
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

internal sealed class UpdateActivityRuleCommandHandler(
    IReadActivityRuleRepository readActivityRuleRepository,
    IWriteActivityRuleRepository writeActivityRuleRepository) : ICommandHandler<UpdateActivityRuleCommand>
{
    public async Task HandleAsync(UpdateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await readActivityRuleRepository.GetByIdAsync(command.Id, cancellationToken);

        if (activityRule is null)
        {
            throw new NotFoundException("UpdateActivityRule.ActivityRule", nameof(activityRule), command.Id.ToString());
        }

        var tmp = activityRule.HasChanges(command.Title, command.Mode, command.SelectedDays);
        if (tmp)
        {
            activityRule.Edit(command.Title, command.Mode, command.SelectedDays);
            await writeActivityRuleRepository.UpdateAsync(activityRule, cancellationToken);
        }
    }
}