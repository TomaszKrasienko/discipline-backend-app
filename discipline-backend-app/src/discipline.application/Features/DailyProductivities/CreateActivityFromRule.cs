

using discipline.application.Behaviours;
using discipline.application.Configuration;
using discipline.application.Exceptions;
using discipline.domain.ActivityRules.Repositories;
using discipline.domain.DailyProductivities.Entities;
using discipline.domain.DailyProductivities.Repositories;
using discipline.domain.SharedKernel.TypeIdentifiers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Extensions = discipline.application.Features.DailyProductivities.Configuration.Extensions;

namespace discipline.application.Features.DailyProductivities;

internal static class CreateActivityFromRule
{
    private const string SectionName = "ActivitiesCronJob";
    // internal static IServiceCollection AddCreateActivityFromRule(this IServiceCollection services, IConfiguration configuration)
    // {
    //     var options = configuration.GetOptions<ActivitiesCronJob>(SectionName);
    //     if (options.IsActive)
    //     {
    //         services.AddQuartz(q =>
    //         {
    //             q.UseMicrosoftDependencyInjectionJobFactory();
    //             var jobKey = new JobKey(options.JobKey);
    //             q.AddJob<CronActivityFromRuleService>(opts => opts.WithIdentity(jobKey));
    //
    //             q.AddTrigger(opts => opts
    //                 .ForJob(jobKey)
    //                 .WithIdentity(options.TriggerKey)
    //                 .WithCronSchedule(options.StartTime.AsQuartzExpression()));
    //         });
    //
    //         services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    //     }
    //
    //     return services;
    // }

    internal static WebApplication MapCreateActivityFromRule(this WebApplication app)
    {
        app.MapPost($"/{Extensions.DailyProductivityTag}/today/add-activity-from-rule", async (CreateActivityFromRuleCommand command,
            CancellationToken cancellationToken, ICommandDispatcher commandDispatcher) =>
        {
            var activityId = ActivityId.New();
            await commandDispatcher.HandleAsync(command with { ActivityId = activityId }, cancellationToken);
            return Results.Ok();
        })
        .Produces(StatusCodes.Status200OK, typeof(void))
        .Produces(StatusCodes.Status400BadRequest, typeof(ErrorDto))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ErrorDto))
        .WithName(nameof(CreateActivityFromRule))
        .WithTags(Extensions.DailyProductivityTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Adds activity rule from activity rule"
        })
        .RequireAuthorization()
        .RequireAuthorization(UserStateCheckingBehaviour.UserStatePolicyName);;
        return app;
    }
}

internal sealed record ActivitiesCronJob
{
    public bool IsActive { get; init; }
    public TimeOnly StartTime { get; init; }
    public string JobKey { get; init; }
    public string TriggerKey { get; init; }
}

internal sealed class CronActivityFromRuleService(IServiceProvider serviceProvider) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = serviceProvider.CreateScope();
        var activityRuleRepository = scope.ServiceProvider.GetRequiredService<IActivityRuleRepository>();
        var commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

        var activities = await activityRuleRepository.BrowseAsync();
        foreach (var activityRule in activities)
        {
            await commandDispatcher.HandleAsync(new CreateActivityFromRuleCommand(ActivityId.New(), activityRule.Id));
        }
    }
}

public sealed record CreateActivityFromRuleCommand(ActivityId ActivityId, ActivityRuleId ActivityRuleId) : ICommand;

public sealed class CreateActivityFromRuleCommandValidator : AbstractValidator<CreateActivityFromRuleCommand>
{
    public CreateActivityFromRuleCommandValidator()
    {
        RuleFor(x => x.ActivityId)
            .Must(activityId => activityId != new ActivityId(Ulid.Empty))
            .WithMessage("\"ActivityId\" can not be empty");
        RuleFor(x => x.ActivityRuleId)
            .Must(activityRuleId => activityRuleId != new ActivityRuleId(Ulid.Empty))
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
            dailyProductivity = DailyProductivity.Create(DailyProductivityId.New(), day, activityRule.UserId);
            dailyProductivity.AddActivityFromRule(command.ActivityId, clock.DateNow(), activityRule);
            await dailyProductivityRepository.AddAsync(dailyProductivity, cancellationToken);
            return;
        }
        
        dailyProductivity.AddActivityFromRule(command.ActivityId, clock.DateNow(), activityRule);
        await dailyProductivityRepository.UpdateAsync(dailyProductivity, cancellationToken);
    }
}