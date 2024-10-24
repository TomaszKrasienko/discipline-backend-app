using discipline.domain.UsersCalendars.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.domain.UsersCalendars.Services.Configuration;

public static class Extensions
{
    public static IServiceCollection AddUserCalendars(this IServiceCollection services)
        => services
            .AddScoped<IChangeEventUserCalendarService, ChangeEventUserCalendarService>();
}