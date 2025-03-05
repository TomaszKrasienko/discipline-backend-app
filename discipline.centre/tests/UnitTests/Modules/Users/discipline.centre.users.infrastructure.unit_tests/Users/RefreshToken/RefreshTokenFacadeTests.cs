using discipline.centre.shared.abstractions.Cache;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth.Configuration;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.infrastructure.Users.Auth.Configuration.Options;
using discipline.centre.users.infrastructure.Users.RefreshToken;
using discipline.centre.users.infrastructure.Users.RefreshToken.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.users.infrastructure.unit_tests.Users.RefreshToken;

public sealed class RefreshTokenFacadeTests
{
    [Fact]
    public async Task GenerateAndSaveAsync_ShouldGenerateRefreshTokenWithProperLength()
    {
        //act
        var result = await _refreshTokenFacade.GenerateAndSaveAsync(UserId.New(), default);
        
        //assert
        result.Length.ShouldBe(_options.Value.Length);
    }
    
    [Fact]
    public async Task GenerateAndSaveAsync_ShouldSaveByICacheFacade()
    {
        //arrange
        var userId = UserId.New();
        
        //act
        var result = await _refreshTokenFacade.GenerateAndSaveAsync(userId, default);
        
        //assert
        await _cacheFacade
            .Received(1)
            .AddAsync(
                userId.ToString(),
                Arg.Is<RefreshTokenDto>(arg => arg.Value == result),
                _options.Value.Expiry);
    }
    
    #region arrange
    private readonly ICacheFacade _cacheFacade;
    private readonly IOptions<RefreshTokenOptions> _options;
    private readonly IRefreshTokenFacade _refreshTokenFacade;

    public RefreshTokenFacadeTests()
    {
        _cacheFacade = Substitute.For<ICacheFacade>();
        _options = Options.Create(new RefreshTokenOptions()
        {
            Length = 20,
            Expiry = new TimeSpan(0, 1, 0, 0)
        });
        _refreshTokenFacade = new RefreshTokenFacade(
            _cacheFacade,
            _options);
    }
    #endregion
}