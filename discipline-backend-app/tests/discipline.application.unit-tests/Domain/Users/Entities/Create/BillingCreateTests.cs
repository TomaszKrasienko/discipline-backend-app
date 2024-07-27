using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Exceptions;
using NSubstitute.ClearExtensions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities.Create;

public sealed class BillingCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnBillingWithFilledFields()
    {
        //arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.Now;
        var isRealized = true;
        var cost = 12m;
        var title = "test_billing_title";
        var cardNumber = new string('2', 14);
        
        //act
        var result = Billing.Create(id, createdAt, isRealized, cost, title, cardNumber);
        
        //assert
        result.Id.Value.ShouldBe(id);
        result.CreatedAt.Value.ShouldBe(createdAt);
        result.IsRealized.Value.ShouldBe(isRealized);
        result.Cost.Value.ShouldBe(cost);
        result.TransferDetails.Title.ShouldBe(title);
        result.TransferDetails.CardNumber.ShouldBe(cardNumber);
    }

    [Fact]
    public void Create_GivenDefaultCreatedAtDateTime_ShouldThrowDefaultCreatedAtException()
    {
        //act
        var exception = Record.Exception(() => Billing.Create(Guid.NewGuid(),
            default, true, 12m, "test_title", new string('1',13)));
        
        //assert
        exception.ShouldBeOfType<DefaultCreatedAtException>();
    }
    
    [Fact]
    public void Create_GivenValueLessThanZero_ShouldThrowBillingValueLessThanZeroException()
    {
        //act
        var exception = Record.Exception(() => Billing.Create(Guid.NewGuid(),
            DateTime.Now, true, -12m, "test_title", new string('1',13)));
        
        //assert
        exception.ShouldBeOfType<BillingValueLessThanZeroException>();
    }

    [Fact]
    public void Create_GivenEmptyTransferDetailsTitle_ShouldThrowEmptyBillingTitleException()
    {
        //act
        var exception = Record.Exception(() => Billing.Create(Guid.NewGuid(),
            DateTime.Now, true, 12m, string.Empty, new string('1', 13)));
        
        //assert
        exception.ShouldBeOfType<EmptyBillingTitleException>();
    }
    
    [Theory]
    [InlineData(11)]
    [InlineData(20)]
    public void Create_GivenInvalidTransferDetailsCardNumberLength_ShouldThrowInvalidCardLengthException(int multiplier)
    {
        //act
        var exception = Record.Exception(() => Billing.Create(Guid.NewGuid(),
            DateTime.Now, true, 12m, "test_billing_title", new string('1', multiplier)));
        
        //assert
        exception.ShouldBeOfType<InvalidCardLengthException>();
    }
}