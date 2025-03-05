using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Subscriptions.Commands.CreateSubscription;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Repositories;
using NSubstitute;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Subscriptions.Commands.CreateSubscription;

public sealed class CreateSubscriptionCommandHandlerTests
{
    private Task Act(CreateSubscriptionCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task Handle_GivenUniqueTitleAndValidaParameters_ShouldAddSubscriptionByRepository()
    {
        //arrange
        var command = new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m, 10m,
            ["test_feature"]);

        _readSubscriptionRepository
            .DoesTitleExistAsync(command.Title, default)
            .Returns(false);
        
        //act
        await Act(command);
        
        //assert
        await _writeSubscriptionRepository
            .Received(1)
            .AddAsync(Arg.Is<Subscription>(arg
                => arg.Id == command.Id
                   && arg.Title == command.Title
                   && arg.Price.PerMonth == command.PricePerMonth
                   && arg.Price.PerYear == command.PricePerYear
                   && arg.Features.Any(x => x == command.Features.Single())), default);
    }
    
    [Fact]
    public async Task Handle_GivenNotUniqueTitle_ShouldNotAddAnySubscriptionByRepository()
    {
        //arrange
        var command = new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m, 10m,
            ["test_feature"]);

        _readSubscriptionRepository
            .DoesTitleExistAsync(command.Title, default)
            .Returns(false);
        
        //act
        await Act(command);
        
        //assert
        await _writeSubscriptionRepository
            .Received(1)
            .AddAsync(Arg.Any<Subscription>());
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCreateSubscription))]
    public async Task Handle_GivenUniqueTitleAndInvalidParameters_ShouldNotAddSubscriptionByRepository(CreateSubscriptionCommand command)
    {
        //arrange
        _readSubscriptionRepository
            .DoesTitleExistAsync(command.Title, default)
            .Returns(false);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        await _writeSubscriptionRepository
            .Received(0)
            .AddAsync(Arg.Any<Subscription>(), default);
    }

    public static IEnumerable<object[]> GetInvalidCreateSubscription()
    {
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), string.Empty, 1m,1m, ["test"])
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", -1m,1m, ["test"])
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m,-1m, ["test"])
        ];
        
        yield return
        [
            new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m,1m, [])
        ];
    }
    
    #region arrange

    private readonly IReadSubscriptionRepository _readSubscriptionRepository;
    private readonly IWriteSubscriptionRepository _writeSubscriptionRepository;
    private readonly ICommandHandler<CreateSubscriptionCommand> _handler;

    public CreateSubscriptionCommandHandlerTests()
    {
        _readSubscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _writeSubscriptionRepository = Substitute.For<IWriteSubscriptionRepository>();
        _handler = new CreateSubscriptionCommandHandler(_readSubscriptionRepository, _writeSubscriptionRepository);
    }
    #endregion
}