using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain.Events;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Events;

public sealed class EventsMapExtensionsTests
{
    [Fact]
    public void MapAsIntegrationEvent_GivenActivityRuleCreated_ShouldMapOnActivityRuleRegisteredEvent()
    {
        //arrange
        var domainEvent = new ActivityRuleCreated(ActivityRuleId.New(), UserId.New());
        
        //act
        var @event = domainEvent.MapAsIntegrationEvent();
        
        //assert
        ((ActivityRuleRegistered)@event).ActivityRuleId.ShouldBe(domainEvent.ActivityRuleId);
        ((ActivityRuleRegistered)@event).UserId.ShouldBe(domainEvent.UserId);
    }
    
    [Fact]
    public void MapAsIntegrationEvent_GivenNotExistingEvent_ShouldThrowInvalidOperationException()
    {
        //arrange
        var domainEvent = new TestEvent();
        
        //act
        var exception = Record.Exception(() => domainEvent.MapAsIntegrationEvent());
        
        //assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }
}

public sealed record TestEvent : DomainEvent;