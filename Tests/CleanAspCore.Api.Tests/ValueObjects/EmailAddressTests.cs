using CleanAspCore.Core.Common.ValueObjects;
using FluentValidation;

namespace CleanAspCore.Api.Tests.ValueObjects;

public sealed class EmailAddressTests
{
    [Fact]
    public void EmailAddress_ValidEmail_CreatesSuccessfully()
    {
        var email = new EmailAddress("user@example.com");
        email.Email.Should().Be("user@example.com");
    }

    [Fact]
    public void EmailAddress_InvalidEmail_ThrowsValidationException()
    {
        var act = () => new EmailAddress("this is not a valid email");
        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().ContainSingle(e => e.PropertyName == "Email");
    }

    [Fact]
    public void EmailAddress_NullEmail_ThrowsValidationException()
    {
        var act = () => new EmailAddress(null!);
        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().ContainSingle(e => e.PropertyName == "Email");
    }
}
