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

namespace discipline.centre.application.unit_tests.Users.Commands.CreateSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandHandlerTests
{
    private Task Act(CreateUserSubscriptionOrderCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenFreeSubscription_ShouldUpdateUserWithFreeSubscriptionOrderByRepository()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, null,null);

        _readUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        //act
        await Act(command);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
        
        await _writeUserRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task HandleAsync_GivenPaidSubscription_ShouldUpdateUserWithPaidSubscriptionOrderByRepository()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            subscription.Id, SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());

        _readUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateTimeNow()
            .Returns(DateTime.UtcNow);
        
        //act
        await Act(command);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        
        await _writeUserRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUser_ShouldThrowNotFoundExceptionWithCodeCreateUserSubscriptionOrderUser()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(UserId.New(), SubscriptionOrderId.New(), 
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("CreateUserSubscriptionOrder.User");
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingSubscription_ShouldThrowNotFoundExceptionWithCodeCreateUserSubscriptionOrderSubscription()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, SubscriptionOrderId.New(), 
            SubscriptionId.New(), SubscriptionOrderFrequency.Monthly, Guid.NewGuid().ToString());

        _readUserRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("CreateUserSubscriptionOrder.Subscription");
    }
    
    #region arrange

    private readonly IReadUserRepository _readUserRepository;
    private readonly IWriteUserRepository _writeUserRepository;
    private readonly IReadSubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionOrderService _subscriptionOrderService;
    private readonly IClock _clock;
    private readonly ICommandHandler<CreateUserSubscriptionOrderCommand> _handler;
    
    public CreateUserSubscriptionOrderCommandHandlerTests()
    {
        _readUserRepository = Substitute.For<IReadUserRepository>();
        _writeUserRepository = Substitute.For<IWriteUserRepository>();
        _subscriptionRepository = Substitute.For<IReadSubscriptionRepository>();
        _subscriptionOrderService = new SubscriptionOrderService();
        _clock = Substitute.For<IClock>();
        _handler = new CreateUserSubscriptionOrderCommandHandler(_readUserRepository, _writeUserRepository, _subscriptionRepository,
            _subscriptionOrderService, _clock);
    }
    #endregion
}