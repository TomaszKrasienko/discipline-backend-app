using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.users.application.Users.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.centre.users.infrastructure.Users.Auth;

internal sealed class JwtAuthenticator(
    IClock clock,
    IOptions<AuthOptions> options) : IAuthenticator
{
    private readonly string _privateCertPath = options.Value.PrivateCertPath;
    private readonly string _password = options.Value.Password;
    private readonly TimeSpan _expiry = options.Value.Expiry;
    private readonly string _issuer = options.Value.Issuer;
    private readonly string _audience = options.Value.Audience;
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
            new Claim("status", status)
        };

        var now = clock.DateTimeNow();
        var expirationTime = now.Add(_expiry);

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now.DateTime,
            expires: expirationTime.DateTime,
            signingCredentials: signingCredentials);

        return _jwtSecurityTokenHandler.WriteToken(jwt);
    }

    private RsaSecurityKey GetPrivateKey()
    {
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromEncryptedPem(input: File.ReadAllText(_privateCertPath), password: _password);
        return new RsaSecurityKey(privateRsa);
    }
}