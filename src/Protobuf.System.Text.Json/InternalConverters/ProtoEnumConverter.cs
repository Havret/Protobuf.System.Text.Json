using System.Text.Encodings.Web;
using System.Text.Json;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class ProtoEnumConverter : InternalConverter
{
    private readonly Dictionary<string, int> _lookup;
    private readonly Dictionary<int, JsonEncodedText> _reversedLookup;
    private readonly Type _clrType;

    public ProtoEnumConverter(EnumDescriptor fieldInfoEnumType, JavaScriptEncoder? encoder)
    {
        _clrType = fieldInfoEnumType.ClrType;
        _lookup = fieldInfoEnumType.Values.ToDictionary(x => x.Name, x => x.Number);
        _reversedLookup = fieldInfoEnumType.Values.ToDictionary(x => x.Number, x => JsonEncodedText.Encode(x.Name, encoder));
    }
    
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var intValue = (int) value;
        if (_reversedLookup.TryGetValue(intValue, out var stringValue))
        {
            writer.WriteStringValue(stringValue);
        }
        else
        {
            writer.WriteNumberValue(intValue);
        }
    }

    public override void Read(ref Utf8JsonReader reader, IMessage obj, Type typeToConvert, JsonSerializerOptions options, IFieldAccessor fieldAccessor)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (reader.GetString() is { } stringValue)
            {
                if (_lookup.TryGetValue(stringValue, out var value))
                {
                    fieldAccessor.SetValue(obj, value);
                    return;
                }

                throw new JsonException($"'{stringValue}' is not a valid value for type {_clrType.FullName}.");
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out var value))
            {
                fieldAccessor.SetValue(obj, value);
            }
        }
    }
}