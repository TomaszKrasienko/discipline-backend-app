using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.users.infrastructure.unit_tests.DAL.Subscriptions;

public sealed class SubscriptionEntitiesMappingExtensionsTests
{
    [Fact]
    public void MapAsDocument_GivenSubscription_ShouldReturnSubscriptionDocument()
    {
        //arrange
        var subscription = SubscriptionFakeDataFactory.Get();
         
        //act
        var document = subscription.MapAsDocument();
         
        //assert
        document.Id.ShouldBe(subscription.Id.ToString());
        document.Title.ShouldBe(subscription.Title.Value);
        document.PricePerMonth.ShouldBe(subscription.Price.PerMonth);
        document.PricePerYear.ShouldBe(subscription.Price.PerYear);
        document.IsPaid.ShouldBe(!subscription.IsFree());
        document.Features.Exists(x => x == subscription.Features.First().Value).ShouldBeTrue();
    }
}