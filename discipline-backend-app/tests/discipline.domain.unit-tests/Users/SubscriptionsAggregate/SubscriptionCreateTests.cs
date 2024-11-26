using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using discipline.domain.Users;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Users.Entities;

public sealed class SubscriptionCreateTests
{
    [Fact]
    public void Create_GivenValidArguments_ShouldReturnSubscriptionWithFilledFields()
    {
        //arrange
        var id = SubscriptionId.New();
        var title = "test_title";
        var pricePerMonth = 10m;
        var pricePerYear = 100m;
        var feature = "test_feature";
        
        //act
        var result = Subscription.Create(id, title, pricePerMonth, pricePerYear,
            [feature]);
        
        //assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Price.PerMonth.ShouldBe(pricePerMonth);
        result.Price.PerYear.ShouldBe(pricePerYear);
        result.Features.Any(x => x.Value == feature).ShouldBeTrue();
    }

    [Fact]
    public void Create_GivenEmptyListOfFeaturs_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(SubscriptionId.New(),
            "test_title", 1, 1, []));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("Subscription.Features.EmptyList");
    }
    
    [Theory]
    [MemberData(nameof(GetSubscriptionCreateInvalidData))]
    public void Create_GivenInvalidArgument_ShouldThrowDomainException(SubscriptionCreateParameters parameters)
    {
        //act
        var exception = Record.Exception(() => Subscription.Create(parameters.SubscriptionId,
            parameters.Title, parameters.PricePerMonth, parameters.PricePerYear, parameters.Features));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }

    public static IEnumerable<object[]> GetSubscriptionCreateInvalidData()
    {
        yield return [new SubscriptionCreateParameters(SubscriptionId.New(),
            string.Empty, 12, 123, ["test"])];
        
        yield return [new SubscriptionCreateParameters(SubscriptionId.New(),
            "test_title", -1m, 1m, ["test"])];
        
        yield return [new SubscriptionCreateParameters(SubscriptionId.New(),
            "test_title", 1m, -1m, ["test"])];
        
        yield return [new SubscriptionCreateParameters(SubscriptionId.New(),
            "test_title", 1, 1, [string.Empty])];
    }
    
    public sealed record SubscriptionCreateParameters(SubscriptionId SubscriptionId,
        string Title, decimal PricePerMonth, decimal PricePerYear, List<string> Features);
}