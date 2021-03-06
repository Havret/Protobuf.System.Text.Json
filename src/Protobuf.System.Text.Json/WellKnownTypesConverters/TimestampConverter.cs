using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;

namespace Protobuf.System.Text.Json.WellKnownTypesConverters;

internal class TimestampConverter : JsonConverter<Timestamp?>
{
    private JsonConverter<DateTime>? _converter;

    public override Timestamp? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && reader.TryGetDateTime(out var dateTime))
        {
            return dateTime.ToTimestamp();
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Timestamp? value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            var dateTime = value.ToDateTime();
            _converter ??= (JsonConverter<DateTime>) options.GetConverter(typeof(DateTime));
            _converter.Write(writer, dateTime, options);
        }
    }
}