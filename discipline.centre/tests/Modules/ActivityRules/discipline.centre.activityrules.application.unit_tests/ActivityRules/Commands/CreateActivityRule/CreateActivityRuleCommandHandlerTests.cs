using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands.CreateActivityRule;

public partial class CreateActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityRuleCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingTitle_ShouldCreateActivityRule()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
        
        _readActivityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(false);

        //act
        await Act(command);
        
        //assert
        await _writeActivityRuleRepository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.Id == command.Id
                   && arg.Title.Value == command.Title
                   && arg.Mode.Value == command.Mode));
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldThrowAlreadyRegisteredExceptionWithCode()
    {
        //arrange
        var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
        _readActivityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(true);

        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<AlreadyRegisteredException>();
        ((AlreadyRegisteredException)exception).Code.ShouldBe("CreateActivityRule.Title");
    }
    
    [Fact]
    public async Task HandleAsync_GivenAlreadyRegisteredRuleTitle_ShouldNotAddAnyActivityRuleByRepository()
    {
        //arrange
       var command = new CreateActivityRuleCommand(ActivityRuleId.New(), UserId.New(), 
            "Rule title", Mode.EveryDayMode, null);
        _readActivityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(true);

        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _writeActivityRuleRepository
            .Received(0)
            .AddAsync(Arg.Any<ActivityRule>(), default);
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateActivityRuleCommand))]
    public async Task HandleAsync_GivenInvalidArgumentsForActivityRule_ShouldNotAddActivityRule(CreateActivityRuleCommand command)
    {
        //arrange
        _readActivityRuleRepository
            .ExistsAsync(command.Title, default)
            .Returns(false);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _writeActivityRuleRepository
            .Received(0)
            .AddAsync(Arg.Any<ActivityRule>(), default);
    }
    
    #region arrange
    private readonly IReadActivityRuleRepository _readActivityRuleRepository;
    private readonly IWriteActivityRuleRepository _writeActivityRuleRepository;
    private readonly ICommandHandler<CreateActivityRuleCommand> _handler;

    public CreateActivityRuleCommandHandlerTests()
    {
        _readActivityRuleRepository = Substitute.For<IReadActivityRuleRepository>();
        _writeActivityRuleRepository = Substitute.For<IWriteActivityRuleRepository>();
        _handler = new CreateActivityRuleCommandHandler(_readActivityRuleRepository,
            _writeActivityRuleRepository);
    }
    #endregion
}