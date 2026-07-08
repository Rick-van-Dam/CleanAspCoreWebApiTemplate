using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanAspCore.Core.Common.ValueObjects;

public sealed class EmailAddressJsonConverter : JsonConverter<EmailAddress>
{
    public override EmailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var email = reader.GetString();
        return new EmailAddress(email!);
    }

    public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
    {
        if (value.Email is null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Email);
    }
}
