using System.Reflection;
using discipline.centre.shared.abstractions.Modules;

namespace discipline.centre.bootstrappers.api;

internal static class ModuleLoader
{
    private const string ModulePartsPrefix = "discipline.centre";
    
    internal static List<Assembly> GetAssemblies(IConfiguration configuration)
    {
        var allAssemblies = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .ToList();

        var allNotDynamicLocations = allAssemblies
            .Where(x => !x.IsDynamic)
            .Select(x => x.Location)
            .ToArray();

        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !allNotDynamicLocations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        var disabledModules = new List<string>();
        foreach (var file in files)
        {
            if (!file.Contains(ModulePartsPrefix))
            {
                continue;
            }

            var moduleName = file.Split(ModulePartsPrefix)[1].Split(".")[0].ToLowerInvariant();
            var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");
            if (enabled)
            {
                disabledModules.Add(file);
            }
        }
        foreach (var module in disabledModules)
        {
            files.Remove(module);
        }
        files.ForEach(x => allAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));
        return allAssemblies;
    }
    
    internal static IList<IModule> GetModules(IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
}