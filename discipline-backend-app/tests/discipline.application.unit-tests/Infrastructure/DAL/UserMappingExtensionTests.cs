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
         ((PaidSubscriptionOrderDocument)result.SubscriptionOrder).Type.ShouldBe((int)subscriptionOrder.Type.Value);
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


     [Fact]
     public void AsEntity_GivenUserDocumentWithPaidSubscriptionOrder_ShouldReturnUserWithPaidSubscriptionOrder()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var paidSubscriptionOrder = PaidSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = paidSubscriptionOrder;
         
         //act
         var result = userDocument.AsEntity();
         
         //assert
         result.Id.Value.ShouldBe(userDocument.Id);
         result.Email.Value.ShouldBe(userDocument.Email);
         result.Password.Value.ShouldBe(userDocument.Password);
         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
         result.FullName.LastName.ShouldBe(userDocument.LastName);
         result.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
         result.SubscriptionOrder.Id.Value.ShouldBe(paidSubscriptionOrder.Id);
         result.SubscriptionOrder.CreatedAt.Value.ShouldBe(paidSubscriptionOrder.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.Value.ShouldBe(paidSubscriptionOrder.SubscriptionId);
         result.SubscriptionOrder.State.ActiveTill.ShouldBe(paidSubscriptionOrder.StateActiveTill);
         result.SubscriptionOrder.State.IsCancelled.ShouldBe(paidSubscriptionOrder.StateIsCancelled);
         ((PaidSubscriptionOrder)result.SubscriptionOrder).Next.Value.ShouldBe(paidSubscriptionOrder.Next);
         ((PaidSubscriptionOrder)result.SubscriptionOrder).PaymentDetails.CardNumber.ShouldBe(paidSubscriptionOrder.PaymentDetailsCardNumber);
         ((PaidSubscriptionOrder)result.SubscriptionOrder).PaymentDetails.CvvCode.ShouldBe(paidSubscriptionOrder.PaymentDetailsCvvCode);
         ((PaidSubscriptionOrder)result.SubscriptionOrder).Type.Value.ShouldBe((SubscriptionOrderFrequency)paidSubscriptionOrder.Type);
     }
     
     [Fact]
     public void AsEntity_GivenUserDocumentWithFreeSubscriptionOrder_ShouldReturnUserWithFreeSubscriptionOrder()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var freeSubscriptionOrderDocument = FreeSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = freeSubscriptionOrderDocument;
         
         //act
         var result = userDocument.AsEntity();
         
         //assert
         result.Id.Value.ShouldBe(userDocument.Id);
         result.Email.Value.ShouldBe(userDocument.Email);
         result.Password.Value.ShouldBe(userDocument.Password);
         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
         result.FullName.LastName.ShouldBe(userDocument.LastName);
         result.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
         result.SubscriptionOrder.Id.Value.ShouldBe(freeSubscriptionOrderDocument.Id);
         result.SubscriptionOrder.CreatedAt.Value.ShouldBe(freeSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.Value.ShouldBe(freeSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.State.ActiveTill.ShouldBe(freeSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.State.IsCancelled.ShouldBe(freeSubscriptionOrderDocument.StateIsCancelled);
     }

     [Fact]
     public void AsEntity_GivenSubscriptionDocument_ShouldReturnSubscription()
     {
         //arrange
         var subscriptionDocument = SubscriptionDocumentFactory.Get();
         
         //act
         var result = subscriptionDocument.AsEntity();
         
         //assert
         result.Id.Value.ShouldBe(subscriptionDocument.Id);
         result.Title.Value.ShouldBe(subscriptionDocument.Title);
         result.Price.PerMonth.ShouldBe(subscriptionDocument.PricePerMonth);
         result.Price.PerYear.ShouldBe(subscriptionDocument.PricePerYear);
     }
}