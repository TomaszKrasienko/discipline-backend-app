using Microsoft.Extensions.DependencyInjection;

namespace discipline.application.Behaviours;

internal static class ClockBehaviour
{
    internal static IServiceCollection AddClockBehaviour(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}

internal interface IClock
{
    DateTime DateNow();
    DateTime DateTimeNow();
}

internal sealed class Clock : IClock
{
    public DateTime DateNow()
        => DateTime.Now.Date;

    public DateTime DateTimeNow()
        => DateTime.Now;
}