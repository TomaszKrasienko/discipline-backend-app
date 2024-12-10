using discipline.centre.dailytrackers.application.ActivityRules.Clients;
using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unit_tests.DailyTrackers.Commands;

public sealed class CreateActivityFromActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityFromActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task Handle_GivenValidCommand_ShouldCreateActivityFromActivityRule()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        _repository
            .DoesActivityWithActivityRuleExistAsync(command.ActivityRuleId, command.UserId, today, default)
            .Returns(false);

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
        
        //act
        
    }
    
    [Fact]
    public async Task Handle_GivenAlreadyExistedActivityForActivityRule_ShouldThrowAlreadyRegisteredException()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        _repository
            .DoesActivityWithActivityRuleExistAsync(command.ActivityRuleId, command.UserId, today, default)
            .Returns(true);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<AlreadyRegisteredException>();
    }

    [Fact]
    public async Task Handle_GivenNotExistingActivityRule_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new CreateActivityFromActivityRuleCommand(ActivityRuleId.New(), UserId.New());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        _clock
            .DateNow()
            .Returns(today);

        _repository
            .DoesActivityWithActivityRuleExistAsync(command.ActivityRuleId, command.UserId, today, default)
            .Returns(false);

        _apiClient
            .GetActivityRuleByIdAsync(command.ActivityRuleId, command.UserId)
            .ReturnsNull();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
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