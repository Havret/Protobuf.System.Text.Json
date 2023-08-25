using System.Text.Json;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Protobuf.System.Text.Json.InternalConverters;

internal class ByteStringConverter : InternalConverter
{
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var base64String = ((ByteString)value).ToBase64();
        writer.WriteStringValue(base64String);
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options,
        IFieldAccessor fieldAccessor)
    {
        var base64String = reader.GetString();
        if (base64String is null)
        {
            return;
        }
        var value = ByteString.FromBase64(base64String);
        fieldAccessor.SetValue(obj, value);
    }
}