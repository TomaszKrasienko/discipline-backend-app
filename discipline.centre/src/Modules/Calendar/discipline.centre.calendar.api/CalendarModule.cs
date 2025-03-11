using discipline.centre.shared.abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.calendar.api;

internal sealed class CalendarModule : IModule
{
    internal const string ModuleName = "calendar-module";
    public string Name => "Calendar";
    
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
    }

    public void Use(WebApplication app)
    {
    }
}