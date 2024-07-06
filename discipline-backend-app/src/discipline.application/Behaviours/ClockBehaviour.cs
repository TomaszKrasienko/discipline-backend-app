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
}

internal sealed class Clock : IClock
{
    public DateTime DateNow()
        => DateTime.Now.Date;
}