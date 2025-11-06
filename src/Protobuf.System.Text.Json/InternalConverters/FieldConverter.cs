using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class FieldConverter<T> : InternalConverter
{
    private JsonConverter<T>? _converter;

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _converter ??= GetConverter(ref options);
        _converter.Write(writer, (T) value, options);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        _converter ??= GetConverter(ref options);
        if (reader.TokenType == JsonTokenType.Null && !_converter.HandleNull)
        {
            return;
        }
        
        var read = _converter.Read(ref reader, typeToConvert, options);
        if (read is { } value)
        {
            fieldAccessor.SetValue(obj, value);
        }
    }
    
    private static JsonConverter<T> GetConverter(ref JsonSerializerOptions options)
    {
        var converter = options.GetConverter(typeof(T));
        if (converter == null)
        {
            throw new JsonException($"There is no converter available for type '{typeof(T)}'.");
        }

        return (JsonConverter<T>) converter;
    }
}