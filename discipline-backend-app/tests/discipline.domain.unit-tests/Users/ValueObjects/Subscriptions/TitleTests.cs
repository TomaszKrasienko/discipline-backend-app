
using discipline.domain.SharedKernel.Exceptions;
using discipline.domain.Users.ValueObjects.Subscriptions;
using Shouldly;
using Xunit;

namespace discipline.domain.unit_tests.Users.ValueObjects.Subscriptions;

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
