using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using Shouldly;
using Xunit;

namespace discipline.centre.users.infrastructure.unit_tests.DAL.Subscriptions;

public sealed class SubscriptionDocumentsMappingExtensionsTests
{
    [Fact]
    public void MapAsEntity_GivenSubscriptionDocument_ShouldReturnSubscription()
    {
        //arrange
        var subscriptionDocument = SubscriptionDocumentFactory.Get();
         
        //act
        var result = subscriptionDocument.MapAsEntity();
         
        //assert
        result.Id.Value.ShouldBe(Ulid.Parse(subscriptionDocument.Id));
        result.Title.Value.ShouldBe(subscriptionDocument.Title);
        result.Price.PerMonth.ShouldBe(subscriptionDocument.PricePerMonth);
        result.Price.PerYear.ShouldBe(subscriptionDocument.PricePerYear);
        result.Features.Any(x => x.Value == subscriptionDocument.Features[0]).ShouldBeTrue();
    }
    
    [Fact]
    public void MapAsDto_GivenSubscriptionDocument_ShouldReturnSubscriptionDto()
    {
        //arrange
        var document = SubscriptionDocumentFactory.Get();
         
        //act
        var dto = document.MapAsDto();
         
        //assert
        dto.Id.ShouldBe(Ulid.Parse(document.Id));
        dto.Title.ShouldBe(document.Title);
        dto.PricePerMonth.ShouldBe(document.PricePerMonth);
        dto.PricePerYear.ShouldBe(document.PricePerYear);
        dto.IsPaid.ShouldBe(document.IsPaid);
        dto.Features.Exists(x => x == document.Features[0]).ShouldBeTrue();
    }
}