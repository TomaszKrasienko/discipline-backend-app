using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.users.domain.Subscriptions.ValueObjects;
using Shouldly;
using Xunit;

namespace discipline.centre.domain.unit_tests.Subscriptions.ValueObjects;

public sealed class TitleTests
{
   [Fact]
   public void Create_GivenValidTitle_ShouldReturnTitleWithValue()
   {
      //arrange
      var value = "test_title";
      
      //act
      var result = Title.Create(value);
      
      //assert
      result.Value.ShouldBe(value);
   }

   [Fact]
   public void Create_GivenEmptyTitle_ShouldThrowDomainExceptionWithEmptyCode()
   {
      //act
      var exception = Record.Exception(() => Title.Create(string.Empty));
      
      //assert
      exception.ShouldBeOfType<DomainException>();
      ((DomainException)exception).Code.ShouldBe("Subscription.Title.Empty");
   }
}
