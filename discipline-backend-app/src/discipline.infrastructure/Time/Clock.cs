using discipline.application.Behaviours.Time;

namespace discipline.infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateOnly DateNow()
        => DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime);

    public DateTimeOffset DateTimeNow()
        => DateTimeOffset.UtcNow;
}