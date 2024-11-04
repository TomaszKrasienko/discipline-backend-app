using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace discipline.centre.shared.abstractions.Modules;

public interface IModule
{
    string Name { get; }
    IEnumerable<string>? Policies => null;
    void Register(IServiceCollection services);
    void Use(WebApplication app);
}