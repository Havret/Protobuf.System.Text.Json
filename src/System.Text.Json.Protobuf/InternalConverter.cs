using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace System.Text.Json.Protobuf;

internal abstract class InternalConverter
{
    public abstract void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options);

    public abstract void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options, IFieldAccessor fieldAccessor);
}