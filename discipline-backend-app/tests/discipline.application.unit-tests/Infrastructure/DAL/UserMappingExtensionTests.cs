using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.application.Infrastructure.DAL.Documents.Users;
using discipline.domain.Users.Entities;
using discipline.domain.Users.Enums;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class UserMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenUserWithoutSubscriptionOrder_ShouldReturnUserDocumentWithSubscriptionOrderAsNull()
    {
        //arrange
        var user = UserFactory.Get();
         
        //act
        var result = user.AsDocument();
         
        //assert
        result.Id.ShouldBe(user.Id.ToString());
        result.Email.ShouldBe(user.Email.Value);
        result.Password.ShouldBe(user.Password.Value);
        result.FirstName.ShouldBe(user.FullName.FirstName);
        result.LastName.ShouldBe(user.FullName.LastName);
        result.Status.ShouldBe(user.Status.Value);
        result.SubscriptionOrder.ShouldBeNull();
    }
    
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
         result.Id.ShouldBe(user.Id.ToString());
         result.Email.ShouldBe(user.Email.Value);
         result.Password.ShouldBe(user.Password.Value);
         result.FirstName.ShouldBe(user.FullName.FirstName);
         result.LastName.ShouldBe(user.FullName.LastName);
         result.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrderDocument>();
         result.SubscriptionOrder.Id.ShouldBe(subscriptionOrder.Id.ToString());
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
         result.Id.ShouldBe(user.Id.ToString());
         result.Email.ShouldBe(user.Email.Value);
         result.Password.ShouldBe(user.Password.Value);
         result.FirstName.ShouldBe(user.FullName.FirstName);
         result.LastName.ShouldBe(user.FullName.LastName);
         result.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrderDocument>();
         result.SubscriptionOrder.Id.ShouldBe(subscriptionOrder.Id.ToString());
         result.SubscriptionOrder.CreatedAt.ShouldBe(now);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(subscription.Id.Value);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(subscriptionOrder.State.IsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBeNull();
     }

     [Fact]
     public void AsEntity_GivenUserDocumentWithoutSubscriptionOrder_ShouldReturnUserWithSubscriptionOrderAsNull()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         
         //act
         var result = userDocument.AsEntity();
         
         //assert
         result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.Value.ShouldBe(userDocument.Email);
         result.Password.Value.ShouldBe(userDocument.Password);
         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
         result.FullName.LastName.ShouldBe(userDocument.LastName);
         result.Status.Value.ShouldBe(userDocument.Status);
         result.SubscriptionOrder.ShouldBeNull();
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
         result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.Value.ShouldBe(userDocument.Email);
         result.Password.Value.ShouldBe(userDocument.Password);
         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
         result.FullName.LastName.ShouldBe(userDocument.LastName);
         result.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
         result.SubscriptionOrder.Id.Value.ShouldBe(Ulid.Parse(paidSubscriptionOrder.Id));
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
         result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.Value.ShouldBe(userDocument.Email);
         result.Password.Value.ShouldBe(userDocument.Password);
         result.FullName.FirstName.ShouldBe(userDocument.FirstName);
         result.FullName.LastName.ShouldBe(userDocument.LastName);
         result.SubscriptionOrder.ShouldBeOfType<FreeSubscriptionOrder>();
         result.SubscriptionOrder.Id.Value.ShouldBe(Ulid.Parse(freeSubscriptionOrderDocument.Id));
         result.SubscriptionOrder.CreatedAt.Value.ShouldBe(freeSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.Value.ShouldBe(freeSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.State.ActiveTill.ShouldBe(freeSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.State.IsCancelled.ShouldBe(freeSubscriptionOrderDocument.StateIsCancelled);
     }

     [Fact]
     public void AsDto_GivenUserDocumentWithoutSubscriptionOrder_ShouldReturnUserDtoWithNullAsSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         
         //act
         var result = userDocument.AsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder.ShouldBeNull();
     }
     
     [Fact]
     public void AsDto_GivenUserDocumentWithFreeSubscriptionOrder_ShouldReturnUserDtoWithSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var freeSubscriptionOrderDocument = FreeSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = freeSubscriptionOrderDocument;
         
         //act
         var result = userDocument.AsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder.Id.ShouldBe(Ulid.Parse(freeSubscriptionOrderDocument.Id));
         result.SubscriptionOrder.CreatedAt.ShouldBe(freeSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(freeSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(freeSubscriptionOrderDocument.StateIsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBe(freeSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.Next.ShouldBeNull();
         result.SubscriptionOrder.PaymentDetailsCardNumber.ShouldBeNull();
         result.SubscriptionOrder.PaymentDetailsCvvCode.ShouldBeNull();
         result.SubscriptionOrder.Type.ShouldBeNull();
     }
     
     [Fact]
     public void AsDto_GivenUserDocumentWithPaidSubscriptionOrder_ShouldReturnUserDtoWithSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var paidSubscriptionOrderDocument = PaidSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = paidSubscriptionOrderDocument;
         
         //act
         var result = userDocument.AsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder.Id.ShouldBe(Ulid.Parse(paidSubscriptionOrderDocument.Id));
         result.SubscriptionOrder.CreatedAt.ShouldBe(paidSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(paidSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(paidSubscriptionOrderDocument.StateIsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBe(paidSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.Next.ShouldBe(paidSubscriptionOrderDocument.Next);
         result.SubscriptionOrder.PaymentDetailsCardNumber.ShouldBe(paidSubscriptionOrderDocument.PaymentDetailsCardNumber);
         result.SubscriptionOrder.PaymentDetailsCvvCode.ShouldBe(paidSubscriptionOrderDocument.PaymentDetailsCvvCode);
         result.SubscriptionOrder.Type.ShouldBe(paidSubscriptionOrderDocument.Type);
     }

     [Fact]
     public void AsEntity_GivenSubscriptionDocument_ShouldReturnSubscription()
     {
         //arrange
         var subscriptionDocument = SubscriptionDocumentFactory.Get();
         
         //act
         var result = subscriptionDocument.AsEntity();
         
         //assert
         result.Id.Value.ShouldBe(Ulid.Parse(subscriptionDocument.Id));
         result.Title.Value.ShouldBe(subscriptionDocument.Title);
         result.Price.PerMonth.ShouldBe(subscriptionDocument.PricePerMonth);
         result.Price.PerYear.ShouldBe(subscriptionDocument.PricePerYear);
         result.Features.Any(x => x.Value == subscriptionDocument.Features[0]).ShouldBeTrue();
     }

     [Fact]
     public void AsDocument_GivenSubscription_ShouldReturnSubscriptionDocument()
     {
         //arrange
         var subscription = SubscriptionFactory.Get();
         
         //act
         var document = subscription.AsDocument();
         
         //assert
         document.Id.ShouldBe(subscription.Id.ToString());
         document.Title.ShouldBe(subscription.Title.Value);
         document.PricePerMonth.ShouldBe(subscription.Price.PerMonth);
         document.PricePerYear.ShouldBe(subscription.Price.PerYear);
         document.IsPaid.ShouldBe(!subscription.IsFreeSubscription());
         document.Features.Any(x => x == subscription.Features.First().Value).ShouldBeTrue();
     }

     [Fact]
     public void AsDto_GivenSubscriptionDocument_ShouldReturnSubscriptionDto()
     {
         //arrange
         var document = SubscriptionDocumentFactory.Get();
         
         //act
         var dto = document.AsDto();
         
         //assert
         dto.Id.ShouldBe(Ulid.Parse(document.Id));
         dto.Title.ShouldBe(document.Title);
         dto.PricePerMonth.ShouldBe(document.PricePerMonth);
         dto.PricePerYear.ShouldBe(document.PricePerYear);
         dto.IsPaid.ShouldBe(document.IsPaid);
         dto.Features.Any(x => x == document.Features[0]).ShouldBeTrue();
     }
}