using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace System.Text.Json.Protobuf;

internal class RepeatedFieldConverter<T> : InternalConverter
{
    private JsonConverter<IList<T>>? _converter;

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _converter ??= (JsonConverter<IList<T>>) options.GetConverter(typeof(IList<T>));
        _converter.Write(writer, (IList<T>) value, options);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        _converter ??= (JsonConverter<IList<T>>) options.GetConverter(typeof(IList<T>));

        var elements = _converter.Read(ref reader, typeof(IList<T>), options);
        if (elements == null)
        {
            return;
        }

        var value = (RepeatedField<T>)fieldAccessor.GetValue(obj);
        value.AddRange(elements);
    }
}