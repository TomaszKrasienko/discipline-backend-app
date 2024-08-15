using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.application.Features.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.domain.Users.Repositories;
using discipline.domain.Users.Services;
using discipline.domain.Users.Services.Abstractions;
using discipline.tests.shared.Entities;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Features.Users.CreateUserSubscriptionOrder;

public sealed class CreateUserSubscriptionOrderCommandHandlerTests
{
    private Task Act(CreateUserSubscriptionOrderCommand command) => _handler.HandleAsync(command, default);

    [Fact]
    public async Task HandleAsync_GivenFreeSubscription_ShouldUpdateUserWithFreeSubscriptionOrderByRepository()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, Guid.NewGuid(),
            subscription.Id, null,null, null);

        _userRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateNow()
            .Returns(DateTime.Now);
        
        //act
        await Act(command);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.Value.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
        
        await _userRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task HandleAsync_GivenPaidSubscription_ShouldUpdateUserWithPaidSubscriptionOrderByRepository()
    {
        //arrange
        var user = UserFactory.Get();
        var subscription = SubscriptionFactory.Get(10, 100);
        var command = new CreateUserSubscriptionOrderCommand(user.Id, Guid.NewGuid(),
            subscription.Id, SubscriptionOrderFrequency.Monthly, new string('1', 14),
            "123");

        _userRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);

        _subscriptionRepository
            .GetByIdAsync(command.SubscriptionId)
            .Returns(subscription);

        _clock
            .DateNow()
            .Returns(DateTime.Now);
        
        //act
        await Act(command);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.Value.ShouldBe(command.Id);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        
        await _userRepository
            .Received(1)
            .UpdateAsync(user);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingUser_ShouldThrowUserNotFoundException()
    {
        //arrange
        var command = new CreateUserSubscriptionOrderCommand(Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), SubscriptionOrderFrequency.Monthly, new string('1', 14),
            "123");
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<UserNotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_GivenNotExistingSubscription_ShouldThrowSubscriptionNotFoundException()
    {
        //arrange
        var user = UserFactory.Get();
        var command = new CreateUserSubscriptionOrderCommand(user.Id, Guid.NewGuid(),
            Guid.NewGuid(), SubscriptionOrderFrequency.Monthly, new string('1', 14),
            "123");

        _userRepository
            .GetByIdAsync(command.UserId)
            .Returns(user);
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<SubscriptionNotFoundException>();
    }
    
    #region arrange
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionOrderService _subscriptionOrderService;
    private readonly IClock _clock;
    private readonly ICommandHandler<CreateUserSubscriptionOrderCommand> _handler;
    
    public CreateUserSubscriptionOrderCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _subscriptionRepository = Substitute.For<ISubscriptionRepository>();
        _subscriptionOrderService = new SubscriptionOrderService();
        _clock = Substitute.For<IClock>();
        _handler = new CreateUserSubscriptionOrderCommandHandler(_userRepository, _subscriptionRepository,
            _subscriptionOrderService, _clock);
    }
    #endregion
}