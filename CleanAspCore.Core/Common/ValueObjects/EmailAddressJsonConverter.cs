using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;

namespace CleanAspCore.Core.Common.ValueObjects;

public sealed class EmailAddressJsonConverter : JsonConverter<EmailAddress>
{
    public override bool HandleNull => true;

    public override EmailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var email = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
        try
        {
            return new EmailAddress(email!);
        }
        catch (ValidationException ex)
        {
            throw new JsonException(ex.Message, ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
    {
        if (value.Email is null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Email);
    }
}
