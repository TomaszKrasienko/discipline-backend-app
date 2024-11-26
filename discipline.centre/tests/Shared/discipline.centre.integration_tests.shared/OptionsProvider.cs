using Microsoft.Extensions.Configuration;

namespace discipline.centre.integration_tests.shared;

internal sealed class OptionsProvider
{
    private readonly IConfiguration _configuration = GetConfigurationRoot();
    
    private static IConfigurationRoot GetConfigurationRoot()
        => new ConfigurationBuilder()
            .AddJsonFile($"appsettings.tests.json")
            .AddEnvironmentVariables()
            .Build();

    internal T Get<T>() where T : class, new()
    {
        var t = new T();
        _configuration.Bind(typeof(T).Name, t);
        return t;
    }
}