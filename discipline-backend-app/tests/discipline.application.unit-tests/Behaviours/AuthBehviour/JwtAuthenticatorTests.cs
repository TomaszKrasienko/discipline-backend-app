using System.IdentityModel.Tokens.Jwt;
using discipline.application.Behaviours;
using discipline.application.Domain.Users.Entities;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Options;
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
            .DateNow()
            .Returns(DateTime.Now);
        var userId = Guid.NewGuid();
        var subscription = "test_subscription";
        var state = "test_state";
        
        //act
        var result = _authenticator.CreateToken(userId.ToString(), subscription, state);
        
        //assert
        result.ShouldNotBeNull();
        var token = _jwtSecurityTokenHandler.ReadJwtToken(result.Token);
        token.Issuer.ShouldBe(_options.Value.Issuer);
        token.Audiences.Single().ShouldBe(_options.Value.Audience);
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