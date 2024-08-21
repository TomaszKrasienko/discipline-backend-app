using discipline.application.Behaviours;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.CryptographyBehaviour;

public sealed class AesCryptographerTests
{
    [Fact]
    public async Task EncryptAsync_GivenStringValue_ShouldReturnEncryptedValue()
    {
        //arrange
        var value = "my_test_value";

        //act
        var result = await _cryptographer.EncryptAsync(value);

        //assert
        result.ShouldNotBe(value);
        var decryptedResult = await _cryptographer.DecryptAsync(result);
        decryptedResult.ShouldBe(value);
    }

    [Fact]
    public async Task DecryptAsync_GivenEmptyValue_ShouldThrowArgumentException()
    {
        //arrange
        var value = string.Empty;
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _cryptographer.DecryptAsync(value));
        
        //assert
        exception.ShouldBeOfType<ArgumentException>();
    }
    
    [Fact]
    public async Task DecryptAsync_GivenEncryptedValue_ShouldReturnDecryptedValue()
    {
        //arrange
        var value = "my_test_value";
        var encryptedValue = await _cryptographer.EncryptAsync(value);
        
        //act
        var decryptedValue = await _cryptographer.DecryptAsync(encryptedValue);
        
        //assert
        decryptedValue.ShouldNotBe(encryptedValue);
        decryptedValue.ShouldBe(value);
    }

    #region arrange
    private readonly ICryptographer _cryptographer;

    public AesCryptographerTests()
        => _cryptographer = new AesCryptographer("icpJrty2W0wtOSuVHuxPaLokVBlrzg6P");

    #endregion
}