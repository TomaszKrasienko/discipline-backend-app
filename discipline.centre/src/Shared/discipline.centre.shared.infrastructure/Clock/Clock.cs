using discipline.centre.shared.abstractions.Clock;

namespace discipline.centre.shared.infrastructure.Clock;

internal sealed class Clock : IClock
{
    public DateTimeOffset DateTimeNow()
        => DateTimeOffset.Now;
}