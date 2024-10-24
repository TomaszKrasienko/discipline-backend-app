using discipline.application.Behaviours;
using discipline.application.Exceptions;
using discipline.domain.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http.Features;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.RefreshTokenPersistenceBehaviour;

public sealed class RefreshTokenFacadeTests
{
    [Fact]
    public async Task GenerateAsync_GivenNotEmptyRefreshTokenAndUserId_ShouldSaveByRefreshTokenServiceAndReturnDecryptedString()
    {
        //arrange
        var userId = UserId.New();
        var encryptedRefreshToken = Guid.NewGuid().ToString();
        var refreshToken = Guid.NewGuid().ToString();
        _cryptographer
            .EncryptAsync(Arg.Any<string>(), default)
            .Returns(encryptedRefreshToken);

        //act
        var result = await _facade.GenerateAsync(userId, default);
        
        //assert
        await _refreshTokenService
            .Received()
            .SaveOrReplaceAsync(Arg.Any<string>(), userId);

        result.ShouldBe(encryptedRefreshToken);
    }
    
    [Fact]
    public async Task GenerateAsync_GivenEmptyUserId_ShouldThrowEmptyUserIdException()
    {
        //arrange
        var userId = new UserId(Ulid.Empty);
        var refreshToken = Guid.NewGuid().ToString();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _facade.GenerateAsync(userId));
        
        //assert
        exception.ShouldBeOfType<EmptyUserIdException>();
    }

    [Fact]
    public async Task GetUserIdAsync_GivenExistingRefreshToken_ShouldReturnUserId()
    {
        //arrange
        var userId = UserId.New();
        var encryptedRefreshToken = Guid.NewGuid().ToString();
        var decryptedRefreshToken = Guid.NewGuid().ToString();

        _cryptographer
            .DecryptAsync(encryptedRefreshToken)
            .Returns(decryptedRefreshToken);

        _refreshTokenService
            .GetAsync(decryptedRefreshToken)
            .Returns(userId);
        
        //act
        var result = await _facade.GetUserIdAsync(encryptedRefreshToken);
        
        //assert
        result.ShouldBe(userId);
    }

    [Fact]
    public async Task GetUserIdAsync_GivenInvalidRefreshToken_ShouldThrowInvalidRefreshTokenException()
    {
        //arrange
        _cryptographer
            .DecryptAsync(Arg.Any<string>(), default)
            .ReturnsNull();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _facade.GetUserIdAsync(Guid.NewGuid().ToString()));
        
        //assert
        exception.ShouldBeOfType<InvalidRefreshTokenException>();
    }
    
    [Fact]
    public async Task GetUserIdAsync_GivenNotExistingRefreshToken_ShouldThrowRefreshTokenForUserNotFoundException()
    {
        //arrange
        var refreshToken = Guid.NewGuid().ToString();
        _cryptographer
            .DecryptAsync(refreshToken)
            .Returns(Guid.NewGuid().ToString());
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _facade.GetUserIdAsync(refreshToken));
        
        //assert
        exception.ShouldBeOfType<RefreshTokenForUserNotFoundException>();
    }
    
    #region arrange
    private readonly ICryptographer _cryptographer;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenFacade _facade;

    public RefreshTokenFacadeTests()
    {
        _cryptographer = Substitute.For<ICryptographer>();
        _refreshTokenService = Substitute.For<IRefreshTokenService>();
        _facade = new RefreshTokenFacade(_cryptographer, _refreshTokenService);
    }
    #endregion
}