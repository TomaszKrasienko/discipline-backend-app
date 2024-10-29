using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace discipline.application.Behaviours;

internal sealed record CryptographyOptions
{
    public string Key { get; init; }
}

public interface ICryptographer
{
    Task<string?> EncryptAsync(string value, CancellationToken cancellationToken = default);
    Task<string?> DecryptAsync(string value, CancellationToken cancellationToken = default);
}
