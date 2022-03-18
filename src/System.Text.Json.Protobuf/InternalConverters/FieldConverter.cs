using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace System.Text.Json.Protobuf.InternalConverters;

internal class FieldConverter<T> : InternalConverter
{
    private JsonConverter<T>? _converter;

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _converter ??= (JsonConverter<T>) options.GetConverter(typeof(T));
        _converter.Write(writer, (T) value, options);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        var jsonConverter = (JsonConverter<T>) options.GetConverter(typeToConvert);
        var read = jsonConverter.Read(ref reader, typeToConvert, options);
        if (read is { } value)
        {
            fieldAccessor.SetValue(obj, value);
        }
    }
}