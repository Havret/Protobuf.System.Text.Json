using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;

namespace Protobuf.System.Text.Json.WellKnownTypesConverters;

internal class ByteStringConverter : JsonConverter<ByteString?>
{
    public override ByteString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.GetString() is { } base64String)
        {
            return ByteString.FromBase64(base64String);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, ByteString? value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            writer.WriteStringValue(value.ToBase64());
        }
    }
}