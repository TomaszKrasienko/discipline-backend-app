using discipline.application.Domain.Exceptions;
using discipline.application.Domain.Policies;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Domain.Policies;

public sealed class MeetingAddressPolicyTests
{
    [Theory]
    [InlineData("test_platform", "test_meeting_address", null)]
    [InlineData(null, "test_meeting_address", null)]
    [InlineData("test_platform", null, null)]
    [InlineData(null, null, "test_place")]
    public void Validate_GivenInvalidArguments_ShouldNotThrowException(string platform, string uri, string place)
    {
        //arrange
        var policy = MeetingAddressPolicy.GetInstance(platform, uri, place);
        
        //act
        var exception = Record.Exception(() => policy.Validate());
        
        //assert
        exception.ShouldBeNull();
    }
    
    [Fact]
    public void Validate_GivenAllEmptyOrNullFields_ShouldReturnEmptyAddressException()
    {
        //arrange
        var policy = MeetingAddressPolicy.GetInstance(string.Empty, null, string.Empty);
        
        //act
        var exception = Record.Exception(() => policy.Validate());
        
        //assert
        exception.ShouldBeOfType<EmptyAddressException>();
    }

    [Theory]
    [InlineData("test_platform", null)]
    [InlineData(null, "test_meeting_address")]
    public void Validate_GivenPlaceAndOnlineMeetingArgument_ShouldReturnInconsistentAddressTypeException(string platform,
        string uri)
    {
        //arrange
        var policy = MeetingAddressPolicy.GetInstance(platform, uri, "test_place");
        
        //act
        var exception = Record.Exception(() => policy.Validate());
        
        //assert
        exception.ShouldBeOfType<InconsistentAddressTypeException>();
    }
}