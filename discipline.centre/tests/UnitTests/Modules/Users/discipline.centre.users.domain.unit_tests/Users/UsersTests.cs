using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.domain.Users.Events;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Users;

public sealed class UsersTests
{
    [Fact]
    public void Create_GivenAllValidArguments_ShouldReturnUserWithAllFillFieldsAndDomainEvent()
    {
        //arrange
        var id = UserId.New();
        var email = "test@test.pl";
        var password = "Test123!";
        var firstName = "test_first_name";
        var lastName = "test_last_name";
        
        //act
        var result = User.Create(id, email, password, firstName, lastName);
        
        //assert
        result.Id.ShouldBe(id);
        result.Email.Value.ShouldBe(email);
        result.Password.Value.ShouldBe(password);
        result.FullName.FirstName.ShouldBe(firstName);
        result.FullName.LastName.ShouldBe(lastName);
        result.Status.Value.ShouldBe("Created");
        result.DomainEvents.Any(x => x is UserCreated).ShouldBeTrue();
    }

    [Theory]
    [MemberData(nameof(GetUserCreateInvalidData))]
    public void Create_GivenInvalidArgument_ShouldThrowDomainException(UserCreateParameters parameters)
    {
        //act
        var exception = Record.Exception(() => User.Create(parameters.UserId, parameters.Email,
            parameters.Password, parameters.FirstName, parameters.LastName));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }

    public static IEnumerable<object[]> GetUserCreateInvalidData()
    {
        yield return [new UserCreateParameters(UserId.New(), string.Empty,
                "test_password", "test_first_name", "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test_email",
            "test_password", "test_first_name", "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", string.Empty, "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", new string('t', 1), "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", new string('t', 101), "test_last_name")];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name",string.Empty)];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name", new string('t', 1))];
        
        yield return [new UserCreateParameters(UserId.New(), "test@test.pl",
            "test_password", "test_first_name", new string('t', 101))];
    }

    public sealed record UserCreateParameters(UserId UserId, string Email, string Password,
        string FirstName, string LastName);
    
    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        //act
        user.CreateFreeSubscriptionOrder(subscriptionOrderId, subscription, DateTime.Now);
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
        user.Status.Value.ShouldBe("FreeSubscriptionPicked");
        user.DomainEvents.Any(x => x is FreeSubscriptionPicked).ShouldBeTrue();
    }

    [Fact]
    public void AddFreeSubscriptionOder_GivenUserWithFreeSubscription_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get();
        user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), subscription, DateTime.Now);
        
        //act
        var exception = Record.Exception(() => user.CreateFreeSubscriptionOrder(SubscriptionOrderId.New(), 
            subscription, DateTime.Now));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("User.SubscriptionOrder.AlreadyPicked");
    }

    [Fact]
    public void AddPaidSubscriptionOrder_GivenUserWithoutSubscriptionOrder_ShouldSetPaidSubscriptionOrder()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        var subscriptionOrderId = SubscriptionOrderId.New();
        
        //act
        user.CreatePaidSubscriptionOrder(subscriptionOrderId, subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "test_payment_token");
        
        //assert
        user.SubscriptionOrder.ShouldNotBeNull();
        user.SubscriptionOrder.Id.ShouldBe(subscriptionOrderId);
        user.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        user.Status.Value.ShouldBe("PaidSubscriptionPicked");
        user.DomainEvents.Any(x => x is PaidSubscriptionPicked).ShouldBeTrue();
    }
    
    [Fact]
    public void AddPaidSubscriptionOrder_GivenUserWithPaidSubscriptionOrder_ShouldThrowSubscriptionOrderForUserAlreadyExistsException()
    {
        //arrange
        var user = UserFakeDataFactory.Get();
        var subscription = SubscriptionFakeDataFactory.Get(10, 100);
        user.CreatePaidSubscriptionOrder(SubscriptionOrderId.New(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "test_payment_token");
        
        //act
        var exception = Record.Exception(() => user.CreatePaidSubscriptionOrder(SubscriptionOrderId.New(), subscription,
            SubscriptionOrderFrequency.Monthly, DateTime.Now, "test_payment_token"));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
}