using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class MapConverter<TKey, TValue> : InternalConverter
{
    private JsonConverter<IDictionary<TKey, TValue>>? _jsonConverter;

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _jsonConverter ??= GetConverter(options);
        _jsonConverter.Write(writer, (IDictionary<TKey, TValue>) value, options);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        _jsonConverter ??= GetConverter(options);

        var elements = _jsonConverter.Read(ref reader, typeToConvert, options);
        if (elements == null)
        {
            return;
        }

        var value = (MapField<TKey,TValue>)fieldAccessor.GetValue(obj);
        value.Add(elements);
    }

    private static JsonConverter<IDictionary<TKey, TValue>> GetConverter(JsonSerializerOptions options)
    {
        var dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeof(TKey), typeof(TValue));
        return (JsonConverter<IDictionary<TKey, TValue>>) options.GetConverter(dictionaryType);
    }
}