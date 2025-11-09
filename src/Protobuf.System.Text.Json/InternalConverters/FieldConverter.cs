using System.Buffers;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class FieldConverter<T> : InternalConverter
{
    private JsonConverter<T>? _converter;
    private readonly bool _isConverterForNumberType;
    private readonly bool _isConverterForFloatingPointType;
    
    public FieldConverter()
    {
        var type = typeof(T);
        type = Nullable.GetUnderlyingType(type) ?? type;
        var typeCode = Type.GetTypeCode(type);
        _isConverterForNumberType = typeCode is >= TypeCode.SByte and <= TypeCode.Decimal;
        _isConverterForFloatingPointType = typeCode is TypeCode.Single or TypeCode.Double;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        _converter ??= GetConverter(ref options);
        
        if (_isConverterForNumberType && (JsonNumberHandling.WriteAsString & options.NumberHandling) != 0)
        {
            writer.WriteStringValue(value.ToString());
        }
        else
        {
            _converter.Write(writer, (T) value, options);
        }
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
            var stringValue = reader.GetString();
            
            // Check if it's a named floating-point literal
            if (_isConverterForFloatingPointType && (stringValue == "NaN" || stringValue == "Infinity" || stringValue == "-Infinity"))
            {
                // Only allow these if the flag is set
                if ((JsonNumberHandling.AllowNamedFloatingPointLiterals & options.NumberHandling) != 0)
                {
                    var value = Convert.ChangeType(stringValue switch
                    {
                        "NaN" => typeToConvert == typeof(float) ? float.NaN : double.NaN,
                        "Infinity" => typeToConvert == typeof(float) ? float.PositiveInfinity : double.PositiveInfinity,
                        "-Infinity" => typeToConvert == typeof(float) ? float.NegativeInfinity : double.NegativeInfinity,
                        _ => throw new JsonException($"Unexpected floating-point literal: {stringValue}")
                    }, typeToConvert);
                    fieldAccessor.SetValue(obj, value);
                    return;
                }
                else
                {
                    // Flag not set, so this should fail
                    throw new JsonException($"The string '{stringValue}' cannot be converted to a number without JsonNumberHandling.AllowNamedFloatingPointLiterals.");
                }
            }
            
            var convertedValue = Convert.ChangeType(stringValue, typeToConvert);
            fieldAccessor.SetValue(obj, convertedValue);
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