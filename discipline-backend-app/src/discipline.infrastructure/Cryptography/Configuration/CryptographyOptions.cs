namespace discipline.infrastructure.Cryptography.Configuration;

public sealed record CryptographyOptions
{
    public string Key { get; init; } = string.Empty;
}