﻿namespace CleanAspCore.Domain;

public readonly record struct EmailAddress
{
    public string Email { get; }

    public EmailAddress(string email)
    {
        Email = email;
        EmailAddressValidator.Instance.ValidateAndThrow(this);
    }

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Email;

    public override string ToString() => Email;

    private sealed class EmailAddressValidator : AbstractValidator<EmailAddress>
    {
        public static readonly AbstractValidator<EmailAddress> Instance = new EmailAddressValidator();

        public EmailAddressValidator()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
        }
    }
}


