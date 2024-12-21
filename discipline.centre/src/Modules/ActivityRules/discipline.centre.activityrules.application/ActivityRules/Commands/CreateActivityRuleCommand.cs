using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using FluentValidation;

namespace discipline.centre.activityrules.application.ActivityRules.Commands;

public sealed record CreateActivityRuleCommand(ActivityRuleId Id, UserId UserId, ActivityRuleDetailsSpecification Details, 
    string Mode, List<int>? SelectedDays, List<StageSpecification>? Stages) : ICommand;
    
public sealed class CreateActivityRuleCommandValidator : AbstractValidator<CreateActivityRuleCommand>
{
    public CreateActivityRuleCommandValidator()
    {
        RuleFor(x => x.Details.Title)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Title\" can not be null or empty");
        
        RuleFor(x => x.Details.Title)
            .MaximumLength(30)
            .WithMessage("Activity rule \"Title\" has invalid length");
        
        RuleFor(x => x.Mode)
            .NotNull()
            .NotEmpty()
            .WithMessage("Activity rule \"Mode\" can not be null or empty");
    }
}

internal sealed class CreateActivityRuleCommandHandler(
    IReadWriteActivityRuleRepository readWriteActivityRuleRepository,
    IEventProcessor eventProcessor) : ICommandHandler<CreateActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var isExists = await readWriteActivityRuleRepository.ExistsAsync(command.Details.Title, command.UserId, cancellationToken);
        if (isExists)
        {
            throw new AlreadyRegisteredException("CreateActivityRule.Title",
                $"Activity rule with title: {command.Details.Title} already registered");
        }

        var activity = ActivityRule.Create(command.Id, command.UserId, command.Details,
            command.Mode, command.SelectedDays, command.Stages);
        await readWriteActivityRuleRepository.AddAsync(activity, cancellationToken);
        await eventProcessor.PublishAsync(activity.DomainEvents.Select(x
            => x.MapAsIntegrationEvent()).ToArray());
    }
}