using System.Reflection;
using discipline.centre.bootstrappers.api.Extensions;
using discipline.centre.shared.abstractions.Modules;
using discipline.centre.users.api;

namespace discipline.centre.bootstrappers.api;

internal static class ModuleLoader
{
    private const string ModulePartsPrefix = "discipline.centre";
    
    internal static List<Assembly> GetAssemblies(IConfiguration configuration, IHostEnvironment environment)
    {
        var allAssemblies = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .ToList();

        var allNotDynamicLocations = allAssemblies
            .Where(x
                => !x.IsDynamic)
            .Select(x => x.Location)
            .ToArray();

        var test = environment.IsTestsEnvironment();
        
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x 
                => !allNotDynamicLocations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .Where(x 
                => x.Contains(ModulePartsPrefix)
                && (environment.IsTestsEnvironment() || (!x.Contains("tests") && !x.Contains("xunit")))) 
            .ToList();
        
        var disabledModules = new List<string>();
        var enabledModules = new List<string>();
        
        foreach (var file in files)
        {
            var fileName = file.Split('/').Last(); 
            
            if (!fileName.Contains(ModulePartsPrefix))
            {
                continue;
            }
            
            var moduleName = fileName.Split(".")[2].ToLowerInvariant();
            var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");
            
            if (enabled)
            {
                enabledModules.Add(moduleName);
                continue;
            }
            
            disabledModules.Add(file);
        }
        
        foreach (var module in disabledModules)
        {
            files.Remove(module);
        }
        
        files.ForEach(x => allAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Modules {string.Join(", ", enabledModules.Distinct())} enabled");
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Module: {string.Join(", ", disabledModules.Distinct())} disabled");
        Console.ResetColor();
        
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