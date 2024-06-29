using discipline.application.Behaviours;
using discipline.application.Domain.Entities;
using discipline.application.Domain.Repositories;
using discipline.application.Exceptions;
using discipline.application.Features.Configuration.Base.Abstractions;
using FluentValidation;

namespace discipline.application.Features.DailyProductivities;

public class CreateActivityFromRule
{
    
}

public sealed record CreateActivityFromRuleCommand(Guid ActivityId, Guid ActivityRuleId) : ICommand;

public sealed class CreateActivityFromRuleCommandValidator : AbstractValidator<CreateActivityFromRuleCommand>
{
    public CreateActivityFromRuleCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .NotEmpty()
            .WithMessage("\"ActivityId\" can not be empty");
        RuleFor(x => x.ActivityRuleId)
            .NotEmpty()
            .WithMessage("\"ActivityRuleId\" can not be empty");
    }
}

internal sealed class CreateActivityFromRuleCommandHandler(
    IClock clock,
    IActivityRuleRepository activityRuleRepository,
    IDailyProductivityRepository dailyProductivityRepository) : ICommandHandler<CreateActivityFromRuleCommand>
{
    public async Task HandleAsync(CreateActivityFromRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await activityRuleRepository.GetByIdAsync(command.ActivityRuleId, cancellationToken);
        if (activityRule is null)
        {
            throw new ActivityRuleNotFoundException(command.ActivityRuleId);
        }

        var day = DateOnly.FromDateTime(clock.DateNow());
        var dailyProductivity = await dailyProductivityRepository.GetByDateAsync(day, cancellationToken);
        if (dailyProductivity is null)
        {
            dailyProductivity = DailyProductivity.Create(day);
            dailyProductivity.AddActivityFromRule(command.ActivityId, clock.DateNow(), activityRule);
            await dailyProductivityRepository.AddAsync(dailyProductivity, cancellationToken);
            return;
        }
        
        dailyProductivity.AddActivityFromRule(command.ActivityId, clock.DateNow(), activityRule);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}