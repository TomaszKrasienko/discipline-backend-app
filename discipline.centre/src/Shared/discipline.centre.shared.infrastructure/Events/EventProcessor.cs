using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.shared.infrastructure.Events;

internal sealed class EventProcessor : IEventProcessor
{
    public Task PublishAsync(params DomainEvent[] events)
    {
        Console.WriteLine("test");
        return Task.CompletedTask;
    }
}