using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.UpdateActivityRule;

public partial class UpdateActivityRuleCommandHandlerTests
{
    private Task Act(UpdateActivityRuleCommand command) => _handler.HandleAsync(command, default);

    [Theory]
    [MemberData(nameof(GetValidUpdateActivityRuleCommand))]
    public async Task HandleAsync_GivenValidData_ShouldUpdateByRepository(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id, UserId = activityRule.UserId}));
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .UpdateAsync(Arg.Is<ActivityRule>(arg 
                => arg.Title == command.Title
                && arg.Mode == command.Mode
                && command.SelectedDays == null || arg.SelectedDays!.Values!.Select(x => (int)x).SequenceEqual(command.SelectedDays!)));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new UpdateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), "test_title",
            Mode.EveryDayMode, null);

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId)
            .ReturnsNull();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }

    [Theory]
    [MemberData(nameof(GetNotChangedUpdateActivityRuleData))]
    public async Task HandleAsync_GivenNotChangedData_ShouldNotUpdateByRepository(ActivityRule activityRule,
        UpdateActivityRuleCommand command)
    {
        //assert
        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId)
            .Returns(activityRule);
        
        //act
        await Act(command);
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .UpdateAsync(Arg.Any<ActivityRule>());
    }

    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public async Task Handle_GivenInvalidArguments_ShouldThrowDomainException(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id, UserId = activityRule.UserId}));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public async Task Handle_GivenInvalidArguments_ShouldNotUpdateByRepository(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id, UserId = command.UserId}));
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .UpdateAsync(Arg.Any<ActivityRule>());
    }
    
    #region arrange;
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly ICommandHandler<UpdateActivityRuleCommand> _handler;

    public UpdateActivityRuleCommandHandlerTests()
    { 
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new UpdateActivityRuleCommandHandler(_readWriteActivityRuleRepository);
    }
    #endregion
}