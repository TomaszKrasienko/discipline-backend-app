using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.ValueObjects;
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
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new UpdateActivityRuleCommand(ActivityRuleId.New(), "test_title",
            Mode.EveryDayMode, null);

        _readActivityRuleRepository
            .GetByIdAsync(command.Id)
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
        _readActivityRuleRepository
            .GetByIdAsync(command.Id)
            .Returns(activityRule);
        
        //act
        await Act(command);
        
        //assert
        await _writeActivityRuleRepository
            .Received(0)
            .UpdateAsync(Arg.Any<ActivityRule>());
    }

    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public async Task Handle_GivenInvalidArguments_ShouldThrowDomainException(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readActivityRuleRepository
            .GetByIdAsync(activityRule.Id)
            .Returns(activityRule);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id }));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidUpdateActivityRuleCommand))]
    public async Task Handle_GivenInvalidArguments_ShouldNotUpdateByRepository(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDateFactory.Get();

        _readActivityRuleRepository
            .GetByIdAsync(activityRule.Id)
            .Returns(activityRule);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id }));
        
        //assert
        await _writeActivityRuleRepository
            .Received(0)
            .UpdateAsync(Arg.Any<ActivityRule>());
    }
    
    #region arrange
    private readonly IReadActivityRuleRepository _readActivityRuleRepository;
    private readonly IWriteActivityRuleRepository _writeActivityRuleRepository;
    private readonly ICommandHandler<UpdateActivityRuleCommand> _handler;

    public UpdateActivityRuleCommandHandlerTests()
    {
        _readActivityRuleRepository = Substitute.For<IReadActivityRuleRepository>();
        _writeActivityRuleRepository = Substitute.For<IWriteActivityRuleRepository>();
        _handler = new UpdateActivityRuleCommandHandler(_readActivityRuleRepository);
    }
    #endregion
}