using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

public static class SharedIHostBuilderConfigurationExtensions
{
    public static IHostBuilder ConfigureModules(this IHostBuilder builder)
        => builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var baseEnvironmentPattern = "module.[A-Za-z]+.json";
            var baseEnvironmentRegex = new Regex(baseEnvironmentPattern);
            
            foreach (var settings in GetSettings("*").Where(x => baseEnvironmentRegex.IsMatch(x)))
            {
                cfg.AddJsonFile(settings);
            }

            foreach (var settings in GetSettings($"*.{ctx.HostingEnvironment.EnvironmentName}"))
            {
                cfg.AddJsonFile(settings);
            }

            IEnumerable<string> GetSettings(string pattern)
                => Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath, $"module.{pattern}.json",
                    SearchOption.AllDirectories);
        });
}