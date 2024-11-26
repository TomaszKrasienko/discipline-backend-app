using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.centre.shared.abstractions.Clock;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.users.infrastructure.unit_tests.Users.Auth;

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
        var email = "test@test.pl";
        var status = "test_state";
        
        //act
        var result = _authenticator.CreateToken(userId.ToString(), email, status);
        
        //assert
        var token = GetTokenFromString(result);
        token.Claims.Single(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value.ShouldBe(userId.ToString());        
        token.Claims.Single(x => x.Type == ClaimTypes.Name).Value.ShouldBe(userId.ToString());
        token.Claims.Single(x => x.Type == CustomClaimTypes.Status).Value.ShouldBe(status);
        token.Claims.Single(x => x.Type == ClaimTypes.Email).Value.ShouldBe(email);
    }

    private JwtSecurityToken GetTokenFromString(string token)
    {
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromPem(input: File.ReadAllText(_options.Value.PublicCertPath));
        
        return _jwtSecurityTokenHandler.ReadJwtToken(token);
    }
    
    [Fact]
    public void CreateToken_GivenUserIdSubscriptionAndState_ShouldReturnValidToken()
    {
        //arrange
        _clock
            .DateTimeNow()
            .Returns(DateTime.Now);
        
        var userId = Guid.NewGuid();
        var email = "test@test.pl";
        var status = "test_state";
        
        //act
        var result = _authenticator.CreateToken(userId.ToString(), email, status);
        
        //assert
        var validationParameters = GetValidationParameters(); 

        var validationResult = _jwtSecurityTokenHandler
            .ValidateToken(result, validationParameters, out SecurityToken validatedToken);
        validationResult.Identity!.IsAuthenticated.ShouldBeTrue();
    }

    private TokenValidationParameters GetValidationParameters()
    {
        RSA privateRsa = RSA.Create();
        privateRsa.ImportFromPem(input: File.ReadAllText(_options.Value.PublicCertPath));
        var publicKey = new RsaSecurityKey(privateRsa);
        
        return new TokenValidationParameters {
            ValidateIssuer = true,
            ValidIssuer = _options.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey,
            ValidateLifetime = true
        };
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
            PublicCertPath = "Certs/public.pem",
            PrivateCertPath = "Certs/private.pem",
            Issuer = "test_issuer",
            Audience = "test_audience",
            Password = "Discipline123!",
            TokenExpiry = TimeSpan.FromMinutes(10)
        });
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        _authenticator = new JwtAuthenticator(_clock, _options);
    }
    #endregion
}