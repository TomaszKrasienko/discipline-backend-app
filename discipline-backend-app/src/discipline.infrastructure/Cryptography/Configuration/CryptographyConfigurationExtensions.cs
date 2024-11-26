using discipline.application.Behaviours;
using discipline.application.Behaviours.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace discipline.infrastructure.Cryptography.Configuration;

internal static class CryptographyBehaviour
{
    private const string SectionName = "Cryptography";

    internal static IServiceCollection AddCryptographyBehaviour(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddServices();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<CryptographyOptions>(configuration.GetSection(SectionName));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddSingleton<ICryptographer>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<AesCryptographer>>();
            var options = sp.GetRequiredService<IOptions<CryptographyOptions>>().Value;
            if (options.Key.Length is not 32)
            {
                throw new ArgumentException("Key has invalid length for cryptography");
            }
            return new AesCryptographer(logger, options.Key);
        });
}