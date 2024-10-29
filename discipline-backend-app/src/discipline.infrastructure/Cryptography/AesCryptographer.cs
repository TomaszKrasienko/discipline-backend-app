using System.Security.Cryptography;
using System.Text;
using discipline.application.Behaviours;
using Microsoft.Extensions.Logging;

namespace discipline.infrastructure.Cryptography;

internal sealed class AesCryptographer(
    ILogger<AesCryptographer> logger, string key) : ICryptographer
{
    public async Task<string?> EncryptAsync(string value, CancellationToken cancellationToken = default)
    {
        using var aes = Aes.Create();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        aes.Key = keyBytes;
        var iv = Convert.ToBase64String(aes.IV);
        var transform = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        await using var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
        await using (var streamWriter = new StreamWriter(cryptoStream))
        {
            await streamWriter.WriteAsync(value);
        }
        return $"{iv}{Convert.ToBase64String(memoryStream.ToArray())}";
    }
    
    public async Task<string?> DecryptAsync(string value, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Data to be decrypted cannot be empty.", nameof(value));
            }

            if (value.Length < 24)
            {
                return null;
            }

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Convert.FromBase64String(value.Substring(0, 24));
            var transform = aes.CreateDecryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream(Convert.FromBase64String(value.Substring(24)));
            await using var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return await streamReader.ReadToEndAsync(cancellationToken);
        }
        catch (FormatException ex)
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
        catch (CryptographicException ex)
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
    }
}