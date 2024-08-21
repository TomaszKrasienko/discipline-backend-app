using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.application.Behaviours;

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
            var options = sp.GetRequiredService<IOptions<CryptographyOptions>>().Value;
            if (options.Key.Length is not 32)
            {
                throw new ArgumentException("Key has invalid length for cryptography");
            }
            return new AesCryptographer(options.Key);
        });
}

internal sealed record CryptographyOptions
{
    public string Key { get; init; }
}

internal interface ICryptographer
{
    Task<string> EncryptAsync(string value, CancellationToken cancellationToken = default);
    Task<string> DecryptAsync(string value, CancellationToken cancellationToken = default);
}

internal sealed class AesCryptographer : ICryptographer
{
    private readonly string _key;

    public AesCryptographer(string key)
    {
        _key = key;
    }
    
    public async Task<string> EncryptAsync(string value, CancellationToken cancellationToken = default)
    {
        using Aes aes = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(_key);
        aes.Key = keyBytes;
        var iv = Convert.ToBase64String(aes.IV);
        var transform = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream memoryStream = new MemoryStream();
        using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            await streamWriter.WriteAsync(value);
        }
        return $"{iv}{Convert.ToBase64String(memoryStream.ToArray())}";
    }

    public async Task<string> DecryptAsync(string value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Data to be decrypted cannot be empty.", nameof(value));
        }

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = Convert.FromBase64String(value.Substring(0, 24));
        var transform = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(Convert.FromBase64String(value.Substring(24)));
        using var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return await streamReader.ReadToEndAsync(cancellationToken);
    }
}