using discipline.centre.shared.abstractions.Modules;
using discipline.centre.users.api.Endpoints;
using discipline.centre.users.infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.api;

internal sealed class UsersModule : IModule
{
    public string Name => "Users";

    public void Register(IServiceCollection services)
        => services.AddInfrastructure(Name);

    public void Use(WebApplication app)
    {
        app.MapUsersEndpoints();
    }
}