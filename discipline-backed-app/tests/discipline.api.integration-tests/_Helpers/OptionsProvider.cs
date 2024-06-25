using Microsoft.Extensions.Configuration;

namespace discipline.api.integration_tests._Helpers;

internal sealed class OptionsProvider
{
    private readonly IConfiguration _configuration = GetConfigurationRoot();
    
    private static IConfigurationRoot GetConfigurationRoot()
        => new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{TestsEnvironmentProvider.GetEnvironments()}.json")
            .AddEnvironmentVariables()
            .Build();

    public T Get<T>(string section) where T : class, new()
    {
        var t = new T();
        _configuration.Bind(section, t);
        return t;
    }
}