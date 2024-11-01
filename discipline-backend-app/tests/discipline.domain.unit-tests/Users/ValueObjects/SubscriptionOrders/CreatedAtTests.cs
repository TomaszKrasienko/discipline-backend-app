using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.SubscriptionOrders;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.SubscriptionOrders;

public sealed class CreatedAtTests
{
    [Fact]
    public void Create_GivenValidArgument_ShouldReturnCreatedAtWithValue()
    {
        //arrange
        var value = DateTimeOffset.UtcNow;
        
        //act
        var result = CreatedAt.Create(value);
        
        //assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_GivenDefaultDateTimeOffset_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => CreatedAt.Create(default));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("SubscriptionOrder.CreatedAt.Default");
    }
}