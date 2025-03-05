using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.domain.Users.Services;
using discipline.centre.users.tests.sharedkernel.Domain;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.application.unit_tests.Users.Commands.CreateSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandHandlerTests
{
    private Task Act(CreateUserSubscriptionOrderCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task GivenFreeSubscription_WhenHandleAsync_ThenShouldAddFreeSubscriptionToUser()
    {
        // Arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, null,null);
            
        _readWriteUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        // Act
        await Act(command);
        
        // Assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
    }
    
    [Fact]
    public async Task GivenFreeSubscription_WhenHandleAsync_ThenShouldUpdateUserByRepository()
    {
        // Arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, null,null);

        _readWriteUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteUserRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task GivenPaidSubscription_WhenHandleAsync_ThenShouldAddPaidSubscriptionToUser()
    {
        // Arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());

        _readWriteUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        // Act
        await Act(command);
        
        // Assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        
        await _readWriteUserRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task GivenPaidSubscription_WhenHandleAsync_ThenShouldUpdateUserByRepository()
    {
        // Arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());

        _readWriteUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteUserRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task GivenNotExistingUser_WhenHandleAsync_ThenShouldThrowNotFoundExceptionWithCodeCreateUserSubscriptionOrderUser()
    {
        // Arrange
        var command = new CreateUserSubscriptionOrderCommand(UserId.New(), SubscriptionOrderId.New(), 
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        // Act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("CreateUserSubscriptionOrder.User");
    }

    [Fact]
    public async Task GivenNotExistingSubscription_WhenHandleAsync_ThenShouldThrowNotFoundExceptionWithCodeCreateUserSubscriptionOrderSubscription()
    {
        // Arrange
        var user = UserFakeDataFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());

        _readWriteUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);
        
        // Act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("CreateUserSubscriptionOrder.Subscription");
    }
    
    #region Arrange
    private readonly IReadWriteUserRepository _readWriteUserRepository;
    private readonly IReadSubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionOrderService _subscriptionOrderService;
    private readonly IClock _clock;
    private readonly ICommandHandler<CreateUserSubscriptionOrderCommand> _handler;
    
    public CreateUserSubscriptionOrderCommandHandlerTests()
    {
        _readWriteUserRepository = Substitute.For<IReadWriteUserRepository>();
        _subscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _subscriptionOrderService = new SubscriptionOrderService();
        _clock = Substitute.For<IClock>();
        _handler = new CreateUserSubscriptionOrderCommandHandler(_readWriteUserRepository, _subscriptionRepository,
            _subscriptionOrderService, _clock);
    }
    #endregion
}