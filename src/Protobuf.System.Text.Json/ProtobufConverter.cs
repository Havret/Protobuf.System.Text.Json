using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Protobuf.System.Text.Json.InternalConverters;

namespace Protobuf.System.Text.Json;

internal class ProtobufConverter<T> : JsonConverter<T?> where T : class, IMessage, new()
{
    private readonly FieldInfo[] _fields;
    private readonly Dictionary<string, FieldInfo> _fieldsLookup;

    public ProtobufConverter(JsonNamingPolicy? namingPolicy, JsonProtobufSerializerOptions options)
    {
        var type = typeof(T);
        
        var propertyTypeLookup = type.GetProperties().ToDictionary(x => x.Name, x => x.PropertyType);

        var propertyInfo = type.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static);
        var messageDescriptor = (MessageDescriptor) propertyInfo?.GetValue(null, null)!;

        var convertNameFunc = GetConvertNameFunc(namingPolicy, options.UseProtobufJsonNames);

        _fields = messageDescriptor.Fields.InDeclarationOrder().Select(fieldDescriptor => new FieldInfo
        {
            Accessor = fieldDescriptor.Accessor,
            IsRepeated = fieldDescriptor.IsRepeated,
            IsMap = fieldDescriptor.IsMap,
            FieldType = GetFieldType(fieldDescriptor, propertyTypeLookup),
            JsonName = convertNameFunc(fieldDescriptor),
            IsOneOf = fieldDescriptor.ContainingOneof != null
        }).ToArray();

        _fieldsLookup = _fields.ToDictionary(x => x.JsonName, x => x);
    }

    private static Func<FieldDescriptor, string> GetConvertNameFunc(JsonNamingPolicy? jsonNamingPolicy, bool useProtobufJsonNames)
    {
        if (useProtobufJsonNames)
        {
            return descriptor => descriptor.JsonName;
        }

        if (jsonNamingPolicy != null)
        {
            return descriptor => jsonNamingPolicy.ConvertName(descriptor.PropertyName);
        }

        return descriptor => descriptor.PropertyName;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var obj = new T();

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"The JSON value could not be converted to {typeToConvert}.");
        }

        // Process all properties.
        while (true)
        {
            // Read the property name or EndObject.
            reader.Read();

            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            var propertyName = reader.GetString();

            if (!_fieldsLookup.TryGetValue(propertyName, out var fieldInfo))
            {
                continue;
            }

            fieldInfo.Converter ??= InternalConverterFactory.Create(fieldInfo);

            reader.Read();
            fieldInfo.Converter.Read(ref reader, obj, fieldInfo.FieldType, options, fieldInfo.Accessor);
        }

        return obj;
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        foreach (var fieldInfo in _fields)
        {
            if (fieldInfo.IsOneOf && fieldInfo.Accessor.HasValue(value) == false)
            {
                continue;
            }

            var obj = fieldInfo.Accessor.GetValue(value);
            if (obj is { } propertyValue)
            {
                writer.WritePropertyName(fieldInfo.JsonName);
                fieldInfo.Converter ??= InternalConverterFactory.Create(fieldInfo);
                fieldInfo.Converter.Write(writer, propertyValue, options);
            }
        }

        writer.WriteEndObject();
    }

    private Type GetFieldType(FieldDescriptor fieldDescriptor, Dictionary<string, Type> propertyTypeLookup)
    {
        switch (fieldDescriptor.FieldType)
        {
            case FieldType.Double:
                return typeof(double);
            case FieldType.Float:
                return typeof(float);
            case FieldType.Int32:
            case FieldType.SInt32:
            case FieldType.SFixed32:
                return typeof(int);
            case FieldType.Int64:
            case FieldType.SInt64:
            case FieldType.SFixed64:
                return typeof(long);
            case FieldType.UInt32:
            case FieldType.Fixed32:
                return typeof(uint);
            case FieldType.UInt64:
            case FieldType.Fixed64:
                return typeof(ulong);
            case FieldType.Bool:
                return typeof(bool);
            case FieldType.String:
                return typeof(string);
            case FieldType.Message when fieldDescriptor.MessageType.ClrType != null:
                return fieldDescriptor.MessageType.ClrType;
            case FieldType.Enum:
            case FieldType.Message:
                return propertyTypeLookup[fieldDescriptor.PropertyName];
            default:
                throw new ArgumentOutOfRangeException(nameof(fieldDescriptor), $"FieldType: '{fieldDescriptor}' is not supported.");
        }
    }
}