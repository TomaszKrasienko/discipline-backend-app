using discipline.application.Infrastructure.DAL.Documents.Mappers;
using discipline.tests.shared.Documents;
using discipline.tests.shared.Entities;
using Shouldly;
using Xunit;

namespace discipline.application.unit_tests.Infrastructure.DAL;

public sealed class UserMappingExtensionsTests
{
    [Fact]
    public void AsDocument_GivenUser_ShouldReturnUserDocument()
    {
        //arrange
        var user = UserFactory.Get();
        
        //act
        var result = user.AsDocument();
        
        //assert
        result.Id.ShouldBe(user.Id.Value);
        result.Email.ShouldBe(user.Email.Value);
        result.Password.ShouldBe(user.Password.Value);
        result.FirstName.ShouldBe(user.FullName.FirstName);
        result.LastName.ShouldBe(user.FullName.LastName);
    }

    [Fact]
    public void AsEntity_GivenUserDocument_ShouldReturnUser()
    {
        //arrange
        var userDocument = UserDocumentFactory.Get();
        
        //act
        var result = userDocument.AsEntity();
        
        //assert
        result.Id.Value.ShouldBe(userDocument.Id);
        result.Email.Value.ShouldBe(userDocument.Email);
        result.Password.Value.ShouldBe(userDocument.Password);
        result.FullName.FirstName.ShouldBe(userDocument.FirstName);
        result.FullName.LastName.ShouldBe(userDocument.LastName);
    }
}