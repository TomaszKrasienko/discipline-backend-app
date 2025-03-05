using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.centre.users.infrastructure.Users.Auth;

internal sealed class JwtAuthenticator(
    IClock clock,
    IOptions<JwtOptions> options) : IAuthenticator
{
    private readonly KeyPublishingOptions _keyPublishingOptions = options.Value.KeyPublishing;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    
    public string CreateToken(string userId, string email, string status)
    {
        var privateKey = GetPrivateKey();
        var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId),
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(CustomClaimTypes.Status, status)
        };

        var now = clock.DateTimeNow();
        var expirationTime = now.Add(_keyPublishingOptions.TokenExpiry);

        var jwt = new JwtSecurityToken(
            issuer: _keyPublishingOptions.Issuer,
            audience: _keyPublishingOptions.Audience,
            claims: claims,
            notBefore: now.DateTime,
            expires: expirationTime.DateTime,
            signingCredentials: signingCredentials);

        return _jwtSecurityTokenHandler.WriteToken(jwt);
    }

    private RsaSecurityKey GetPrivateKey()
    {
        RSA privateRsa = RSA.Create();
        var keyText = File.ReadAllText(_keyPublishingOptions.PrivateCertPath);
        privateRsa.ImportFromEncryptedPem(input: keyText, password: _keyPublishingOptions.PrivateCertPassword);
        return new RsaSecurityKey(privateRsa);
    }
}