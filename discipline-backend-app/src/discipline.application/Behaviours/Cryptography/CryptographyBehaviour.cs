namespace discipline.application.Behaviours.Cryptography;


public interface ICryptographer
{
    Task<string?> EncryptAsync(string value, CancellationToken cancellationToken = default);
    Task<string?> DecryptAsync(string value, CancellationToken cancellationToken = default);
}
