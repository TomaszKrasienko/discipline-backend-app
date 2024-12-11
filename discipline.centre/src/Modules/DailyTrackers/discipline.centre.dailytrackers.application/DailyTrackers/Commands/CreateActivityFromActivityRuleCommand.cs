using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CreateActivityFromActivityRuleCommand(ActivityRuleId ActivityRuleId, UserId UserId) : ICommand;

internal sealed class CreateActivityFromActivityRuleCommandHandler(
    IClock clock,
    IActivityRulesApiClient apiClient,
    IWriteReadDailyTrackerRepository repository) : ICommandHandler<CreateActivityFromActivityRuleCommand>
{
    public async Task HandleAsync(CreateActivityFromActivityRuleCommand command, CancellationToken cancellationToken = default)
    {
        var activityRule = await apiClient.GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId);
        
        if (activityRule is null)
        {
            throw new NotFoundException("CreateActivityFromActivityRuleCommand.ActivityRule",
                nameof(ActivityRuleDto), command.ActivityRuleId.ToString());
        }   

        var today = clock.DateNow();
        var dailyTracker = await repository.GetDailyTrackerByDayAsync(today, command.UserId, cancellationToken);

        List<StageSpecification>? stages = null;
        if (activityRule.Stages is not null)
        {
            stages = [];
            foreach (var stage in activityRule.Stages.OrderBy(x => x.Index))
            {
                stages.Add(new StageSpecification(stage.Title, stage.Index));
            }
        }
        
        if (dailyTracker is not null)
        {
            dailyTracker.AddActivity(new ActivityDetailsSpecification(activityRule.Title, activityRule.Note),
                command.ActivityRuleId, stages);
            await repository.UpdateAsync(dailyTracker, cancellationToken);
            return;
        }
        
        dailyTracker = DailyTracker.Create(DailyTrackerId.New(), today, command.UserId,
            new ActivityDetailsSpecification(activityRule.Title, activityRule.Note),
            command.ActivityRuleId, stages);
        await repository.AddAsync(dailyTracker, cancellationToken);
    }
    
}