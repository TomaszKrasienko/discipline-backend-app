using discipline.application.Behaviours;
using NSubstitute;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.RefreshTokenPersistenceBehaviour;

public sealed class RefreshTokenFacadeTests
{
    [Fact]
    public void Save_GivenNotEmptyRefreshTokenAndNotExistingUser_ShouldSaveByRefreshTokenService()
    {
        
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