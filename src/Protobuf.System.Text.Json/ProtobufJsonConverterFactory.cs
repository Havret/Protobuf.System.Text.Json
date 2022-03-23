using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;

namespace Protobuf.System.Text.Json;

internal class ProtobufJsonConverterFactory : JsonConverterFactory
{
    private readonly JsonProtobufSerializerOptions _options;

    public ProtobufJsonConverterFactory(JsonProtobufSerializerOptions options)
    {
        _options = options;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(IMessage).IsAssignableFrom(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions jsonSerializerOptions)
    {
        var converterType = typeof(ProtobufConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter) Activator.CreateInstance(converterType, new object[] {jsonSerializerOptions, _options})!;
    }
}