using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Protobuf.System.Text.Json.Benchmark.Shared;

public class NewtonsoftProtobufJsonConverter : JsonConverter
{
    private readonly JsonFormatter _jsonFormatter;
    private readonly JsonParser _jsonParser;

    public NewtonsoftProtobufJsonConverter(int messageRecursionLimit = 3, bool formatDefaultValues = true) : this(messageRecursionLimit,
        formatDefaultValues, false)
    {
    }

    public NewtonsoftProtobufJsonConverter(int messageRecursionLimit = 3, bool formatDefaultValues = true,
        bool formatEnumsAsIntegers = false)
    {
        var parserSettings = new JsonParser.Settings(messageRecursionLimit).WithIgnoreUnknownFields(true);
        _jsonParser = new JsonParser(parserSettings);
        var formatterSettings = new JsonFormatter.Settings(formatDefaultValues).WithFormatEnumsAsIntegers(formatEnumsAsIntegers);
        _jsonFormatter = new JsonFormatter(formatterSettings);
    }

    public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteRawValue(_jsonFormatter.Format((IMessage?) value));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        return _jsonParser.Parse(JObject.Load(reader).ToString(), ExtractMessageDescriptor(objectType));
    }

    public override bool CanConvert(Type objectType) => typeof(IMessage).IsAssignableFrom(objectType);

    public override bool CanRead => true;

    private static MessageDescriptor ExtractMessageDescriptor(Type objectType)
    {
        if (objectType is null)
            throw new ArgumentNullException(nameof(objectType));

        var descriptorProperty = objectType
            .GetProperty("Descriptor", BindingFlags.Public | BindingFlags.Static);

        if (descriptorProperty is null)
            throw new NullReferenceException($"There's not descriptor field on {nameof(objectType)} type");

        var descriptorPropertyValue = descriptorProperty
            .GetValue(null, null);

        if (descriptorPropertyValue is null)
            throw new NullReferenceException("There's no value for the descriptor property");

        return (MessageDescriptor) descriptorPropertyValue;
    }
}