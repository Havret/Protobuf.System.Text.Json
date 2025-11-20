using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class FieldConverter<T> : InternalConverter
{
    private JsonConverter<T>? _converter;
    private readonly bool _isConverterForNumberType;
    
    public FieldConverter()
    {
        var type = typeof(T);
        type = Nullable.GetUnderlyingType(type) ?? type;
        var typeCode = Type.GetTypeCode(type);
        _isConverterForNumberType = typeCode is >= TypeCode.SByte and <= TypeCode.Decimal;
    }

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

        if (_isConverterForNumberType && reader.TokenType == JsonTokenType.String && (JsonNumberHandling.AllowReadingFromString & options.NumberHandling) != 0)
        {
            var value = Convert.ChangeType(reader.GetString(), typeToConvert, CultureInfo.InvariantCulture);
            fieldAccessor.SetValue(obj, value);
        }
        else
        {
            var read = _converter.Read(ref reader, typeToConvert, options);
            if (read is { } value)
            {
                fieldAccessor.SetValue(obj, value);
            }
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