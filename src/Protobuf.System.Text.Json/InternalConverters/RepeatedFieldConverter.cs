using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class RepeatedFieldConverter<T> : InternalConverter
{
    private JsonConverter<IList<T>>? _converter;

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _converter ??= GetConverter(ref options);
        _converter.Write(writer, (IList<T>) value, options);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        _converter ??= GetConverter(ref options);

        var elements = _converter.Read(ref reader, typeof(IList<T>), options);
        if (elements == null)
        {
            return;
        }

        var value = (RepeatedField<T>)fieldAccessor.GetValue(obj);
        value.AddRange(elements);
    }
    
    private static JsonConverter<IList<T>> GetConverter(ref JsonSerializerOptions options)
    {
        var converter = options.GetConverter(typeof(IList<T>));
        if (converter == null)
        {
            throw new JsonException($"There is no converter available for type '{typeof(IList<T>)}'.");
        }

        return (JsonConverter<IList<T>>) converter;
    }
}