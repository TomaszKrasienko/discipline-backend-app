// using discipline.application.Domain.Users.Entities;
// using discipline.application.Domain.Users.Enums;
// using discipline.application.Infrastructure.DAL.Documents.Mappers;
// using discipline.tests.shared.Documents;
// using discipline.tests.shared.Entities;
// using Shouldly;
// using Xunit;
//
// namespace discipline.application.unit_tests.Infrastructure.DAL;
//
// public sealed class UserMappingExtensionsTests
// {
//     [Fact]
//     public void AsDocument_GivenUser_ShouldReturnUserDocument()
//     {
//         //arrange
//         var user = UserFactory.Get();
//         var subscription = SubscriptionFactory.Get();
//         var subscriptionOrder = SubscriptionOrderFactory.Get(subscription);
//         // user.CreateSubscriptionOrder(subscriptionOrder.Id, subscription,
//         //     subscriptionOrder.);
//         
//         //act
//         var result = user.AsDocument();
//         
//         //assert
//         result.Id.ShouldBe(user.Id.Value);
//         result.Email.ShouldBe(user.Email.Value);
//         result.Password.ShouldBe(user.Password.Value);
//         result.FirstName.ShouldBe(user.FullName.FirstName);
//         result.LastName.ShouldBe(user.FullName.LastName);
//     }
//
//     [Fact]
//     public void AsEntity_GivenUserDocument_ShouldReturnUser()
//     {
//         //arrange
//         var userDocument = UserDocumentFactory.Get();
//         
//         //act
//         var result = userDocument.AsEntity();
//         
//         //assert
//         result.Id.Value.ShouldBe(userDocument.Id);
//         result.Email.Value.ShouldBe(userDocument.Email);
//         result.Password.Value.ShouldBe(userDocument.Password);
//         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
//         result.FullName.LastName.ShouldBe(userDocument.LastName);
//     }
//
//     [Fact]
//     public void AsEntity_GivenSubscriptionDocument_ShouldReturnSubscription()
//     {
//         //arrange
//         var subscriptionDocument = SubscriptionDocumentFactory.Get();
//         
//         //act
//         var result = subscriptionDocument.AsEntity();
//         
//         //assert
//         result.Id.Value.ShouldBe(subscriptionDocument.Id);
//         result.Title.Value.ShouldBe(subscriptionDocument.Title);
//         result.Price.PerMonth.ShouldBe(subscriptionDocument.PricePerMonth);
//         result.Price.PerYear.ShouldBe(subscriptionDocument.PricePerYear);
//     }
// }