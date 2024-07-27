using discipline.application.Domain.Users.Entities;
using discipline.application.Domain.Users.Enums;
using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class UserMappingExtensionsTests
{
     [Fact]
     public void AsDocument_GivenUserWithPaidSubscriptionOrder_ShouldReturnUserDocumentWithPaidSubscriptionOrderDocument()
     {
         //arrange
         var user = UserFactory.Get();
         var subscription = SubscriptionFactory.Get(10, 100);
         var subscriptionOrder = PaidSubscriptionOrderFactory.Get(subscription);
         var now = DateTime.Now;
         user.CreatePaidSubscriptionOrder(subscriptionOrder.Id, subscription,
             subscriptionOrder.Type.Value, now, subscriptionOrder.PaymentDetails.CardNumber,
             subscriptionOrder.PaymentDetails.CvvCode);
         
         //act
         var result = user.AsDocument();
         
         //assert
         result.Id.ShouldBe(user.Id.Value);
         result.Email.ShouldBe(user.Email.Value);
         result.Password.ShouldBe(user.Password.Value);
         result.FirstName.ShouldBe(user.FullName.FirstName);
         result.LastName.ShouldBe(user.FullName.LastName);
         result.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrderDocument>();
         result.SubscriptionOrder.Id.ShouldBe(subscriptionOrder.Id.Value);
         result.SubscriptionOrder.CreatedAt.ShouldBe(now);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(subscription.Id.Value);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(subscriptionOrder.State.IsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBe(subscriptionOrder.State.ActiveTill);
         ((PaidSubscriptionOrderDocument)result.SubscriptionOrder).Next.ShouldBe(subscriptionOrder.Next.Value);
         ((PaidSubscriptionOrderDocument)result.SubscriptionOrder).PaymentDetailsCardNumber.ShouldBe(subscriptionOrder.PaymentDetails.CardNumber);
         ((PaidSubscriptionOrderDocument)result.SubscriptionOrder).PaymentDetailsCvvCode.ShouldBe(subscriptionOrder.PaymentDetails.CvvCode);
     }
     
     [Fact]
     public void AsDocument_GivenUserWithFreeSubscriptionOrder_ShouldReturnUserDocumentWithFreeSubscriptionOrderDocument()
     {
         //arrange
         var user = UserFactory.Get();
         var subscription = SubscriptionFactory.Get();
         var subscriptionOrder = FreeSubscriptionOrderFactory.Get(subscription);
         var now = DateTime.Now;
         user.CreateFreeSubscriptionOrder(subscriptionOrder.Id, subscription, now);
         
         //act
         var result = user.AsDocument();
         
         //assert
         result.Id.ShouldBe(user.Id.Value);
         result.Email.ShouldBe(user.Email.Value);
         result.Password.ShouldBe(user.Password.Value);
         result.FirstName.ShouldBe(user.FullName.FirstName);
         result.LastName.ShouldBe(user.FullName.LastName);
         result.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrderDocument>();
         result.SubscriptionOrder.Id.ShouldBe(subscriptionOrder.Id.Value);
         result.SubscriptionOrder.CreatedAt.ShouldBe(now);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(subscription.Id.Value);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(subscriptionOrder.State.IsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBeNull();
     }


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
}