using System.Text.Json.Serialization;
using Google.Protobuf;

namespace System.Text.Json.Protobuf;

internal class ProtobufJsonConverterFactory : JsonConverterFactory
{
    private readonly JsonProtobufSerializerOptions _options;

    public ProtobufJsonConverterFactory(JsonProtobufSerializerOptions options)
    {
        _options = options;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(IMessage));
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(ProtobufConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter) Activator.CreateInstance(converterType, new object[]{options.PropertyNamingPolicy, _options})!;
    }
}