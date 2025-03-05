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
    public async Task GivenUniqueTitleAndValidParameters_WhenHandleAsync_ShouldAddSubscriptionByRepository()
    {
        // Arrange
        var command = new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m, 10m,
            ["test_feature"]);

        _readWriteSubscriptionRepository
            .DoesTitleExistAsync(command.Title, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteSubscriptionRepository
            .Received(1)
            .AddAsync(Arg.Is<Subscription>(arg
                => arg.Id == command.Id
                   && arg.Title == command.Title
                   && arg.Price.PerMonth == command.PricePerMonth
                   && arg.Price.PerYear == command.PricePerYear
                   && arg.Features.Any(x => x == command.Features.Single())), default);
    }
    
    [Fact]
    public async Task GivenNotUniqueTitle_WhenHandleAsync_ShouldNotAddAnySubscriptionByRepository()
    {
        // Arrange
        var command = new CreateSubscriptionCommand(SubscriptionId.New(), "test_title", 1m, 10m,
            ["test_feature"]);

        _readWriteSubscriptionRepository
            .DoesTitleExistAsync(command.Title, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteSubscriptionRepository
            .Received(1)
            .AddAsync(Arg.Any<Subscription>());
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCreateSubscription))]
    public async Task GivenUniqueTitleAndInvalidParameters_WhenHandleAsync_ShouldNotAddSubscriptionByRepository(CreateSubscriptionCommand command)
    {
        // Arrange
        _readWriteSubscriptionRepository
            .DoesTitleExistAsync(command.Title, CancellationToken.None)
            .Returns(false);
        
        // Act
        _ = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        await _readWriteSubscriptionRepository
            .Received(0)
            .AddAsync(Arg.Any<Subscription>(), CancellationToken.None);
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
    
    #region Arrange

    private readonly IReadWriteSubscriptionRepository _readWriteSubscriptionRepository;
    private readonly ICommandHandler<CreateSubscriptionCommand> _handler;

    public CreateSubscriptionCommandHandlerTests()
    {
        _readWriteSubscriptionRepository = Substitute.For<IReadWriteSubscriptionRepository>();
        _handler = new CreateSubscriptionCommandHandler(_readWriteSubscriptionRepository);
    }
    #endregion
}