using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace discipline.centre.shared.abstractions.Modules;

public interface IModule
{
    string Name { get; }
    IEnumerable<string>? Policies => null;
    void Register(IServiceCollection services, IConfiguration configuration);
    void Use(WebApplication app);
}