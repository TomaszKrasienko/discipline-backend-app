using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using discipline.application.Behaviours;
using discipline.application.Behaviours.Time;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.AuthBehviour;

public sealed class JwtAuthenticatorTests
{
    [Fact]
    public void CreateToken_GivenUserIdSubscriptionAndState_ShouldReturnTokenDtoWithTokenContainingAllClaims()
    {
        //arrange
        _clock
            .DateTimeNow()
            .Returns(DateTime.Now);
        var userId = Guid.NewGuid();
        var status = "test_state";
        
        //act
        var result = _authenticator.CreateToken(userId.ToString(), status);
        
        //assert
        result.ShouldNotBeNull();
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromPem(input: File.ReadAllText(_options.Value.PublicCertPath));
        var publicKey = new RsaSecurityKey(privateRsa);
        var token = _jwtSecurityTokenHandler.ReadJwtToken(result);
        
        token.Claims.FirstOrDefault(x => x.Type == "status")?.Value.ShouldBe(status);
        
        var validationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = _options.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey,
            ValidateLifetime = true
        };

        _jwtSecurityTokenHandler.ValidateToken(result, validationParameters, out SecurityToken validatedToken);
        
    }
    
    #region arrange

    private readonly IClock _clock;
    private readonly IOptions<AuthOptions> _options;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly IAuthenticator _authenticator;

    public JwtAuthenticatorTests()
    {
        _clock = Substitute.For<IClock>();
        _options = Options.Create(new AuthOptions()
        {
            PublicCertPath = "_certs/public.pem",
            PrivateCertPath = "_certs/private.pem",
            Issuer = "test_issuer",
            Audience = "test_audience",
            Password = "Discipline123!",
            Expiry = TimeSpan.FromMinutes(10)
        });
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        _authenticator = new JwtAuthenticator(_clock, _options);
    }

    #endregion
}