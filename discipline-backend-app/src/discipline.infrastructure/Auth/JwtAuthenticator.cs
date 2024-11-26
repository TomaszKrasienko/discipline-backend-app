

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.application.Behaviours;
using discipline.application.Behaviours.Auth;
using discipline.application.Behaviours.Time;
using discipline.infrastructure.Auth.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.infrastructure.Auth;

internal sealed class JwtAuthenticator : IAuthenticator
{
    private readonly IClock _clock;
    private readonly string _privateCertPath;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _password;
    private readonly TimeSpan _expiry;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtAuthenticator(IClock clock, IOptions<AuthOptions> options)
    {
        _clock = clock;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _privateCertPath = options.Value.PrivateCertPath;
        _password = options.Value.Password;
        _expiry = options.Value.Expiry;
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    
    public string CreateToken(string userId, string status)
    {
        SigningCredentials signingCredentials;
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromEncryptedPem(input: File.ReadAllText(_privateCertPath), password: _password);
        var privateKey = new RsaSecurityKey(privateRsa);
        signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);  
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim("status", status)
        };

        var now = _clock.DateTimeNow();
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
}