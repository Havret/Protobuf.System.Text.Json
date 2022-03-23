using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;

namespace Protobuf.System.Text.Json.WellKnownTypesConverters;

internal class DurationConverter : JsonConverter<Duration?>
{
    private JsonConverter<TimeSpan>? _converter;

    public override Duration? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        _converter ??= (JsonConverter<TimeSpan>) options.GetConverter(typeof(TimeSpan));
        var timeSpan = _converter.Read(ref reader, typeToConvert, options);
        return timeSpan.ToDuration();
    }

    public override void Write(Utf8JsonWriter writer, Duration? value, JsonSerializerOptions options)
    {
        if (value != null)
        {
            var timeSpan = value.ToTimeSpan();
            _converter ??= (JsonConverter<TimeSpan>) options.GetConverter(typeof(TimeSpan));
            _converter.Write(writer, timeSpan, options);
        }
    }
}