using discipline.api.integration_tests._Helpers;
using Xunit;

namespace discipline.api.integration_tests.Users;

public sealed class SignUpTests : BaseTestsController
{
    [Fact]
    public async Task SignUp_GivenAlreadyExistingEmail_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        
    }
}