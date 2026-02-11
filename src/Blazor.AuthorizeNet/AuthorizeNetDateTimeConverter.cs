using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blazor.AuthorizeNet;

public class AuthorizeNetDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateTimeFormat = "M/d/yyyy h:mm:ss tt";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        return string.IsNullOrEmpty(dateString) ? DateTime.UtcNow : DateTime.ParseExact(dateString, DateTimeFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
    }
}