using discipline.centre.activityrules.domain.ValueObjects;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects;

public partial class SelectedDaysTests
{
    [Theory]
    [MemberData(nameof(GetSelectedDaysValidData))]
    public void Create_GivenValueInRange_ShouldReturnSelectedDayWithValue(List<int> values)
    {
        //act
        var result = SelectedDays.Create(values);
        
        //assert
        result.Values.Select(x => (int)x).SequenceEqual(values).ShouldBeTrue();
    }
    
    [Theory]
    [MemberData(nameof(GetSelectedDaysInvalidData))]
    public void Create_GivenValueOutOfRange_ThrowDomainExceptionWithCodeActivityRuleSelectedDayOutOfRange(List<int> values)
    {
        //act
        var exception = Record.Exception(() => SelectedDays.Create(values));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.SelectedDay.OutOfRange");
    }
}