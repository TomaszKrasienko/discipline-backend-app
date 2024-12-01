using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateActivityRuleCommand(ActivityRuleId Id, UserId UserId, string Title, string? Note, 
    string Mode, List<int>? SelectedDays) : ICommand;
    
public sealed class CreateActivityRuleCommandValidator : AbstractValidator<CreateActivityRuleCommand>
{
    public CreateActivityRuleCommandValidator()
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

internal sealed class CreateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository) : ICommandHandler<CreateActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var isExists = await readWriteActivityRuleRepository.ExistsAsync(command.Title, command.UserId, cancellationToken);
        if (isExists)
        {
            throw new AlreadyRegisteredException("CreateActivityRule.Title",
                $"Activity rule with title: {command.Title} already registered");
        }

        var activity = ActivityRule.Create(command.Id, command.UserId, command.Title, 
            command.Note, command.Mode, command.SelectedDays);
        await readWriteActivityRuleRepository.AddAsync(activity, cancellationToken);
    }
}