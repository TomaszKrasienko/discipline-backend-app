using discipline.centre.users.domain.Users;
using discipline.centre.users.domain.Users.Enums;
using discipline.centre.users.infrastructure.DAL.Documents;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using Shouldly;
using Xunit;

namespace discipline.centre.users.infrastructure.unit_tests.DAL.Users;

public sealed class UserDocumentsMappingExtensionsTests
{
    [Fact]
    public void MapAsEntity_GivenUserDocumentWithoutSubscriptionOrder_ShouldReturnUserWithSubscriptionOrderAsNull()
    {
        //arrange
        var userDocument = UserDocumentFactory.Get();

        //act
        var result = userDocument.MapAsEntity();

        //assert
        result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
        result.Email.Value.ShouldBe(userDocument.Email);
        result.Password.HashedValue.ShouldBe(userDocument.Password);
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
        var result = userDocument.MapAsEntity();

        //assert
        result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
        result.Email.Value.ShouldBe(userDocument.Email);
        result.Password.HashedValue.ShouldBe(userDocument.Password);
        result.FullName.FirstName.ShouldBe(userDocument.FirstName);
        result.FullName.LastName.ShouldBe(userDocument.LastName);
        result.SubscriptionOrder.ShouldBeOfType<PaidSubscriptionOrder>();
        result.SubscriptionOrder.Id.Value.ShouldBe(Ulid.Parse(paidSubscriptionOrder.Id));
        result.SubscriptionOrder.CreatedAt.Value.ShouldBe(paidSubscriptionOrder.CreatedAt);
        result.SubscriptionOrder.SubscriptionId.Value.ShouldBe(paidSubscriptionOrder.SubscriptionId);
        result.SubscriptionOrder.State.ActiveTill.ShouldBe(paidSubscriptionOrder.StateActiveTill);
        result.SubscriptionOrder.State.IsCancelled.ShouldBe(paidSubscriptionOrder.StateIsCancelled);
        ((PaidSubscriptionOrder)result.SubscriptionOrder).Next.Value.ShouldBe(paidSubscriptionOrder.Next);
        ((PaidSubscriptionOrder)result.SubscriptionOrder).PaymentDetails.Token.ShouldBe(paidSubscriptionOrder.PaymentToken);
        ((PaidSubscriptionOrder)result.SubscriptionOrder).Type.Value.ShouldBe((SubscriptionOrderFrequency)paidSubscriptionOrder.Type);
    }

    [Fact]
    public void MapAsEntity_GivenUserDocumentWithFreeSubscriptionOrder_ShouldReturnUserWithFreeSubscriptionOrder()
    {
        //arrange
        var userDocument = UserDocumentFactory.Get();
        var freeSubscriptionOrderDocument = FreeSubscriptionOrderDocumentFactory.Get();
        userDocument.SubscriptionOrder = freeSubscriptionOrderDocument;

        //act
        var result = userDocument.MapAsEntity();

        //assert
        result.Id.Value.ShouldBe(Ulid.Parse(userDocument.Id));
        result.Email.Value.ShouldBe(userDocument.Email);
        result.Password.HashedValue.ShouldBe(userDocument.Password);
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
     public void MapAsDto_GivenUserDocumentWithoutSubscriptionOrder_ShouldReturnUserDtoWithNullAsSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         
         //act
         var result = userDocument.MapAsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder.ShouldBeNull();
     }
     
     [Fact]
     public void MapAsDto_GivenUserDocumentWithFreeSubscriptionOrder_ShouldReturnUserDtoWithSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var freeSubscriptionOrderDocument = FreeSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = freeSubscriptionOrderDocument;
         
         //act
         var result = userDocument.MapAsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder!.Id.ShouldBe(Ulid.Parse(freeSubscriptionOrderDocument.Id));
         result.SubscriptionOrder.CreatedAt.ShouldBe(freeSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(freeSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(freeSubscriptionOrderDocument.StateIsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBe(freeSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.Next.ShouldBeNull();
         result.SubscriptionOrder.PaymentToken.ShouldBeNull();
         result.SubscriptionOrder.Type.ShouldBeNull();
     }
     
     [Fact]
     public void MapAsDto_GivenUserDocumentWithPaidSubscriptionOrder_ShouldReturnUserDtoWithSubscriptionOrderDto()
     {
         //arrange
         var userDocument = UserDocumentFactory.Get();
         var paidSubscriptionOrderDocument = PaidSubscriptionOrderDocumentFactory.Get();
         userDocument.SubscriptionOrder = paidSubscriptionOrderDocument;
         
         //act
         var result = userDocument.MapAsDto();
         
         //assert
         result.Id.ShouldBe(Ulid.Parse(userDocument.Id));
         result.Email.ShouldBe(userDocument.Email);
         result.FirstName.ShouldBe(userDocument.FirstName);
         result.LastName.ShouldBe(userDocument.LastName);
         result.Status.ShouldBe(userDocument.Status);
         result.SubscriptionOrder!.Id.ShouldBe(Ulid.Parse(paidSubscriptionOrderDocument.Id));
         result.SubscriptionOrder.CreatedAt.ShouldBe(paidSubscriptionOrderDocument.CreatedAt);
         result.SubscriptionOrder.SubscriptionId.ShouldBe(paidSubscriptionOrderDocument.SubscriptionId);
         result.SubscriptionOrder.StateIsCancelled.ShouldBe(paidSubscriptionOrderDocument.StateIsCancelled);
         result.SubscriptionOrder.StateActiveTill.ShouldBe(paidSubscriptionOrderDocument.StateActiveTill);
         result.SubscriptionOrder.Next.ShouldBe(paidSubscriptionOrderDocument.Next);
         result.SubscriptionOrder.PaymentToken.ShouldBe(paidSubscriptionOrderDocument.PaymentToken);
         result.SubscriptionOrder.Type.ShouldBe(paidSubscriptionOrderDocument.Type);
     }
}