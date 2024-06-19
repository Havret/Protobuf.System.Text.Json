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
    private readonly JsonIgnoreCondition _defaultIgnoreCondition;

    public ProtobufConverter(JsonSerializerOptions jsonSerializerOptions, JsonProtobufSerializerOptions jsonProtobufSerializerOptions)
    {
        _defaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;

        var type = typeof(T);
        
        var propertyTypeLookup = type.GetProperties().ToDictionary(x => x.Name, x => x.PropertyType);

        var propertyInfo = type.GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static);
        var messageDescriptor = (MessageDescriptor) propertyInfo?.GetValue(null, null)!;
        
        var convertNameFunc = GetConvertNameFunc(jsonSerializerOptions.PropertyNamingPolicy, jsonProtobufSerializerOptions.UseProtobufJsonNames);

        _fields = messageDescriptor.Fields.InDeclarationOrder().Select(fieldDescriptor =>
        {
            var enumType = jsonProtobufSerializerOptions.UseStringProtoEnumValueNames && fieldDescriptor.FieldType == FieldType.Enum
                ? fieldDescriptor.EnumType
                : null;
            var fieldInfo = new FieldInfo
            {
                Accessor = fieldDescriptor.Accessor,
                IsRepeated = fieldDescriptor.IsRepeated,
                EnumType = enumType,
                IsMap = fieldDescriptor.IsMap,
                FieldType = FieldTypeResolver.ResolverFieldType(fieldDescriptor, propertyTypeLookup),
                JsonName = convertNameFunc(fieldDescriptor),
                IsOneOf = fieldDescriptor.ContainingOneof != null,
            };
            fieldInfo.Converter = InternalConverterFactory.Create(fieldInfo, jsonSerializerOptions);
            return fieldInfo;
        }).ToArray();

        var stringComparer = jsonSerializerOptions.PropertyNameCaseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        _fieldsLookup = _fields.ToDictionary(x => x.JsonName, x => x, stringComparer);
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

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"The JSON value could not be converted to {typeToConvert}.");
        }
        
        var obj = new T();

        // Process all properties.
        while (true)
        {
            // Read the property name or EndObject.
            reader.Read();

            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                reader.Read();
                continue;
            }

            var propertyName = reader.GetString();
            
            if (propertyName == null || !_fieldsLookup.TryGetValue(propertyName, out var fieldInfo))
            {
                // We need to call TrySkip instead of Skip as Skip may throw exception when called in DeserializeAsync
                // context https://github.com/dotnet/runtime/issues/39795
                _ = reader.TrySkip();
                continue;
            }

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
                if (_defaultIgnoreCondition is JsonIgnoreCondition.Never or not JsonIgnoreCondition.WhenWritingDefault)
                {
                    writer.WritePropertyName(fieldInfo.JsonName);
                    fieldInfo.Converter.Write(writer, propertyValue, options); 
                }
            }
            else if (obj is null && _defaultIgnoreCondition == JsonIgnoreCondition.Never)
            {
                writer.WritePropertyName(fieldInfo.JsonName);
                writer.WriteNullValue();
            }
        }

        writer.WriteEndObject();
    }
}