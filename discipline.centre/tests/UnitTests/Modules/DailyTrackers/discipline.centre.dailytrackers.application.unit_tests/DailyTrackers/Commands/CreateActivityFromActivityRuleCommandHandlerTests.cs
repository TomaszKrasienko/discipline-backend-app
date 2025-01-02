using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class CreateActivityFromActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityFromActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task? Handle_GivenValidCommandAndExistingDailyTracker_ShouldAddActivityAndUpdate()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityId.New(), ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        var activityRuleDto = new ActivityRuleDto()
        {
            ActivityRuleId = command.ActivityRuleId,
            Title = "test_activity_rule_title",
            Note = null,
            Mode = "test_mode",
            SelectedDays = null,
            Stages = null
        };
        
        _apiClient
            .GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId)
            .Returns(activityRuleDto);
  
        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), today, command.UserId, ActivityId.New(),
            new ActivityDetailsSpecification("test_title", null), null, null);

        _repository
            .GetDailyTrackerByDayAsync(today, command.UserId, default)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        await _repository
            .Received(1)
            .UpdateAsync(dailyTracker);

        dailyTracker
            .Activities.Any(x
                => x.Id == command.ActivityId 
               && x.ParentActivityRuleId == command.ActivityRuleId
               && x.Details.Title == activityRuleDto.Title
               && x.Details.Note == activityRuleDto.Note).ShouldBeTrue();
        
        await _repository
            .Received(0)
            .AddAsync(Arg.Any<DailyTracker>(), default);
    }
    
    [Fact]
    public async Task? Handle_GivenValidCommandAndNotExistingDailyTracker_ShouldCreateActivityFromActivityRule()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityId.New(), ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        var activityRuleDto = new ActivityRuleDto()
        {
            ActivityRuleId = command.ActivityRuleId,
            Title = "test_activity_rule_title",
            Note = "test_activity_rule_note",
            Mode = "test_mode",
            SelectedDays = null,
            Stages =
            [
                new StageDto()
                {
                    StageId = StageId.New(),
                    Title = "test_stage_title",
                    Index = 1
                }
            ]
        };
        
        _apiClient
            .GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId)
            .Returns(activityRuleDto);

        _repository
            .GetDailyTrackerByDayAsync(today, command.UserId, default)
            .ReturnsNull();
        
        //act
        await Act(command);
        
        //assert
        await _repository
            .Received(1)
            .AddAsync(Arg.Is<DailyTracker>(arg
                => arg.Day.Value == today
                   && arg.UserId == command.UserId
                   && arg.Activities.Any(x 
                       => x.Id == command.ActivityId 
                       && x.Details.Title == activityRuleDto.Title
                       && x.Details.Note == activityRuleDto.Note
                       && x.ParentActivityRuleId == activityRuleDto.ActivityRuleId
                       && x.Stages![0].Title == activityRuleDto.Stages[0].Title
                       && x.Stages![0].Index == activityRuleDto.Stages[0].Index)
            ));
        
        await _repository
            .Received(0)
            .UpdateAsync(Arg.Any<DailyTracker>(), default);
    }
    
    [Fact]
    public async Task Handle_GivenNotExistingActivityRule_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityId.New(), ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        _repository
            .GetDailyTrackerByDayAsync(today, command.UserId, default)
            .ReturnsNull();

        _apiClient
            .GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId)
            .ReturnsNull();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_GivenAlreadyExistedActivityForActivityRule_ShouldThrowAlreadyRegisteredException()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityId.New(), ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        var activityRuleDto = new ActivityRuleDto()
        {
            ActivityRuleId = command.ActivityRuleId,
            Title = "test_activity_rule_title",
            Note = "test_activity_rule_note",
            Mode = "test_mode",
            SelectedDays = null,
            Stages =
            [
                new StageDto()
                {
                    StageId = StageId.New(),
                    Title = "test_stage_title",
                    Index = 1
                }
            ]
        };
        
        _apiClient
            .GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId)
            .Returns(activityRuleDto);

        var dailyTracker = DailyTracker.Create(DailyTrackerId.New(), today, command.UserId, ActivityId.New(),
            new ActivityDetailsSpecification(activityRuleDto.Title, activityRuleDto.Note),
            activityRuleDto.ActivityRuleId, null);
        
        _clock
            .DateNow()
            .Returns(today);
        
        _repository
            .GetDailyTrackerByDayAsync(today, command.UserId, default)
            .Returns(dailyTracker);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
    
    #region arrange
    private readonly IClock _clock;
    private readonly IActivityRulesApiClient _apiClient;
    private readonly IWriteReadDailyTrackerRepository _repository;
    private readonly ICommandHandler<CreateActivityFromActivityRuleCommand> _handler;

    public CreateActivityFromActivityRuleCommandHandlerTests()
    {
        _clock = Substitute.For<IClock>();
        _apiClient = Substitute.For<IActivityRulesApiClient>();
        _repository = Substitute.For<IWriteReadDailyTrackerRepository>();
        _handler = new CreateActivityFromActivityRuleCommandHandler(_clock, _apiClient, _repository);
    }
    #endregion
}