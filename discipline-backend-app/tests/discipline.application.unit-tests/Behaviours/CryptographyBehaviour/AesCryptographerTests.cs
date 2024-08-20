using discipline.application.Behaviours;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Behaviours.CryptographyBehaviour;

public sealed class AesCryptographerTests
{
    [Fact]
    public async Task Encrypt_GivenStringValue_ShouldReturnEncryptedValue()
    {
        //arrange
        var value = "my_test_value";

        //act
        var result = await _cryptographer.Encrypt(value);

        //assert
        result.ShouldNotBe(value);
        var decryptedResult = await _cryptographer.Decrypt(result);
        decryptedResult.ShouldBe(value);
    }

    [Fact]
    public async Task Decrypt_GivenEmptyValue_ShouldThrowArgumentException()
    {
        //arrange
        var value = string.Empty;
        
        //act
        var exception = await Record.ExceptionAsync(async () => await _cryptographer.Decrypt(value));
        
        //assert
        exception.ShouldBeOfType<ArgumentException>();
    }
    
    [Fact]
    public async Task Decrypt_GivenEncryptedValue_ShouldReturnDecryptedValue()
    {
        //arrange
        var value = "my_test_value";
        var encryptedValue = await _cryptographer.Encrypt(value);
        
        //act
        var decryptedValue = await _cryptographer.Decrypt(encryptedValue);
        
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